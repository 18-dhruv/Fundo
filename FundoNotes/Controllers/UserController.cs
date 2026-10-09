using BusinessLayer.Constants;
using BusinessLayer.Exceptions;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models.Common;
using ModelLayer.Models.Users;

namespace FundoNotes.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserBL _userBL;

        public UserController(IUserBL userBL)
        {
            _userBL = userBL;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            var user = await _userBL.RegisterAsync(model);
            return StatusCode(StatusCodes.Status201Created,
                ResponseModel<UserResponseModel>.Success(user, "Registration successful."));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var result = await _userBL.LoginAsync(model);
            return Ok(ResponseModel<LoginResponseModel>.Success(result, "Login successful."));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            await _userBL.ForgotPassworxdAsync(model);
            return Ok(ResponseModel<object>.SuccessWithoutData(
                "If that email is registered, a reset token has been sent."));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            await _userBL.ResetPasswordAsync(model);
            return Ok(ResponseModel<object>.SuccessWithoutData("Password has been reset. You can log in now."));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userBL.GetAllAsync();
            return Ok(ResponseModel<List<UserResponseModel>>.Success(users));
        }

        [Authorize]
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetById(int userId)
        {
            var user = await _userBL.GetByIdAsync(userId);
            return Ok(ResponseModel<UserResponseModel>.Success(user));
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var user = await _userBL.GetByIdAsync(CurrentUserId);
            return Ok(ResponseModel<UserResponseModel>.Success(user));
        }

        // Users can only change or delete their own account, so the id comes from the token, not the URL.
        [Authorize]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMe(UpdateUserModel model)
        {
            var user = await _userBL.UpdateAsync(CurrentUserId, model);
            return Ok(ResponseModel<UserResponseModel>.Success(user, "Profile updated."));
        }

        [Authorize]
        [HttpDelete("me")]
        public async Task<IActionResult> DeleteMe()
        {
            await _userBL.DeleteAsync(CurrentUserId);
            return Ok(ResponseModel<object>.SuccessWithoutData("Account deleted."));
        }

        private int CurrentUserId =>
            int.TryParse(User.FindFirst(ClaimNames.UserId)?.Value, out var id)
                ? id
                : throw new UnauthorizedException("Invalid token.");
    }
}
