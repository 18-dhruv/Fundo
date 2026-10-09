namespace ModelLayer.Models.Users
{
    /// <summary>
    /// What the API exposes about a user. Never contains the password or reset token.
    /// </summary>
    public class UserResponseModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ContactNo { get; set; } = string.Empty;
    }
}
