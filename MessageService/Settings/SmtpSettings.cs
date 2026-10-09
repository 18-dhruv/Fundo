namespace MessageService.Settings
{
    /// <summary>
    /// Bound from the "Smtp" section. Put Username/Password in user secrets.
    /// </summary>
    public class SmtpSettings
    {
        public const string SectionName = "Smtp";

        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 587;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FromName { get; set; } = "Fundoo Notes";
        public string FromEmail { get; set; } = string.Empty;
    }
}
