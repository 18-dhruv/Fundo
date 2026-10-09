namespace BusinessLayer.Interface
{
    /// <summary>
    /// Talks to the separate MessageService over HTTP.
    /// </summary>
    public interface IMessageClient
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
