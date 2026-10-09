using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Models.Users
{
    /// <summary>
    /// Profile fields a user may change. Email and password have their own flows.
    /// </summary>
    public class UpdateUserModel
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact number is required")]
        [RegularExpression(@"^[6-9][0-9]{9}$", ErrorMessage = "Contact number must be a valid 10-digit mobile number")]
        public string ContactNo { get; set; } = string.Empty;
    }
}
