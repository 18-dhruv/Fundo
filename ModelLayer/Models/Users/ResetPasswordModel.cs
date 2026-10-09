using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Models.Users
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Token is required")]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required")]
        [MinLength(6, ErrorMessage = "Password must contain at least 6 characters")]
        public string NewPassword { get; set; } = string.Empty;
    }
}
