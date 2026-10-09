namespace BusinessLayer.Settings
{
    /// <summary>
    /// Bound from the "Jwt" section of appsettings.json / user secrets.
    /// </summary>
    public class JwtSettings
    {
        public const string SectionName = "Jwt";

        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;

        /// <summary>HMAC-SHA256 needs at least 32 characters. Keep it in user secrets.</summary>
        public string Key { get; set; } = string.Empty;

        public int ExpiryMinutes { get; set; } = 60;
    }
}
