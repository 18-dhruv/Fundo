using System.Net;
using System.Security.Cryptography;
using System.Text;
using BusinessLayer.Exceptions;
using BusinessLayer.Interface;
using BusinessLayer.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Models.Users;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private const int ResetTokenValidMinutes = 15;

        private readonly IUserRL _userRL;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IMessageClient _messageClient;

        public UserBL(
            IUserRL userRL,
            IPasswordHasher<UserEntity> passwordHasher,
            IJwtService jwtService,
            IMessageClient messageClient)
        {
            _userRL = userRL;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _messageClient = messageClient;
        }

        public async Task<UserResponseModel> RegisterAsync(RegistrationModel model)
        {
            var email = NormalizeEmail(model.Email);

            if (await _userRL.EmailExistsAsync(email))
            {
                throw new ConflictException("An account with this email already exists.");
            }

            var user = new UserEntity
            {
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
                Email = email,
                ContactNo = model.ContactNo
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

            try
            {
                await _userRL.AddAsync(user);
            }
            catch (DbUpdateException)
            {
                // Two requests with the same email at the same moment: the unique index catches it.
                throw new ConflictException("An account with this email already exists.");
            }

            return user.ToResponse();
        }

        public async Task<LoginResponseModel> LoginAsync(LoginModel model)
        {
            var user = await _userRL.GetByEmailAsync(NormalizeEmail(model.Email));

            // Same message for "no such email" and "wrong password", so attackers can't probe which emails exist.
            if (user is null)
            {
                throw new UnauthorizedException("Invalid email or password.");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedException("Invalid email or password.");
            }

            if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
                await _userRL.UpdateAsync(user);
            }

            var (token, expiresAt) = _jwtService.GenerateToken(user);

            return new LoginResponseModel
            {
                Token = token,
                ExpiresAt = expiresAt,
                User = user.ToResponse()
            };
        }

        public async Task<List<UserResponseModel>> GetAllAsync()
        {
            var users = await _userRL.GetAllAsync();
            return users.Select(u => u.ToResponse()).ToList();
        }

        public async Task<UserResponseModel> GetByIdAsync(int userId)
        {
            var user = await GetExistingUserAsync(userId);
            return user.ToResponse();
        }

        public async Task<UserResponseModel> UpdateAsync(int userId, UpdateUserModel model)
        {
            var user = await GetExistingUserAsync(userId);

            user.FirstName = model.FirstName.Trim();
            user.LastName = model.LastName.Trim();
            user.ContactNo = model.ContactNo;

            await _userRL.UpdateAsync(user);
            return user.ToResponse();
        }

        public async Task DeleteAsync(int userId)
        {
            if (!await _userRL.DeleteAsync(userId))
            {
                throw new NotFoundException("User not found.");
            }
        }

        public async Task ForgotPasswordAsync(ForgotPasswordModel model)
        {
            var user = await _userRL.GetByEmailAsync(NormalizeEmail(model.Email));

            // Don't reveal whether the email is registered. The controller returns the same message either way.
            if (user is null)
            {
                return;
            }

            var rawToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));

            // Only the hash is stored. If the database leaks, the tokens are useless.
            user.ResetTokenHash = HashToken(rawToken);
            user.ResetTokenExpiry = DateTime.UtcNow.AddMinutes(ResetTokenValidMinutes);
            await _userRL.UpdateAsync(user);

            var body =
                $"<p>Hi {WebUtility.HtmlEncode(user.FirstName)},</p>" +
                $"<p>Use this token to reset your Fundoo password. It expires in {ResetTokenValidMinutes} minutes.</p>" +
                $"<p><b>{rawToken}</b></p>" +
                "<p>If you didn't request this, you can ignore this email.</p>";

            await _messageClient.SendEmailAsync(user.Email, "Reset your Fundoo password", body);
        }

        public async Task ResetPasswordAsync(ResetPasswordModel model)
        {
            var user = await _userRL.GetByResetTokenHashAsync(HashToken(model.Token));

            if (user is null || user.ResetTokenExpiry is null || user.ResetTokenExpiry < DateTime.UtcNow)
            {
                throw new BadRequestException("Reset token is invalid or has expired.");
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);

            // One-time use: clear the token so it can't be replayed.
            user.ResetTokenHash = null;
            user.ResetTokenExpiry = null;

            await _userRL.UpdateAsync(user);
        }

        private async Task<UserEntity> GetExistingUserAsync(int userId)
        {
            return await _userRL.GetByIdAsync(userId)
                   ?? throw new NotFoundException("User not found.");
        }

        private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();

        private static string HashToken(string token) =>
            Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token.Trim().ToUpperInvariant())));
    }
}
