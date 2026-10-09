namespace ModelLayer.Models.Users
{
    public class LoginResponseModel
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public UserResponseModel User { get; set; } = new();
    }
}
