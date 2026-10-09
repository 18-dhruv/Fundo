using ModelLayer.Models.Users;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Returns DTOs only. Failures are thrown as FundooException subclasses.
    /// </summary>
    public interface IUserBL
    {
        Task<UserResponseModel> RegisterAsync(RegistrationModel model);

        Task<LoginResponseModel> LoginAsync(LoginModel model);

        Task<List<UserResponseModel>> GetAllAsync();

        Task<UserResponseModel> GetByIdAsync(int userId);

        Task<UserResponseModel> UpdateAsync(int userId, UpdateUserModel model);

        Task DeleteAsync(int userId);

        Task ForgotPasswordAsync(ForgotPasswordModel model);

        Task ResetPasswordAsync(ResetPasswordModel model);
    }
}
