namespace BusinessLayer.Settings
{
    /// <summary>
    /// Bound from the "MessageService" section of appsettings.json.
    /// </summary>
    public class MessageServiceSettings
    {
        public const string SectionName = "MessageService";

        public string BaseUrl { get; set; } = string.Empty;
    }
}
