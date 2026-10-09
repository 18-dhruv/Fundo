using System.Net.Http.Json;
using BusinessLayer.Exceptions;
using BusinessLayer.Interface;

namespace BusinessLayer.Service
{
    /// <summary>
    /// Typed HttpClient. Its BaseAddress (the MessageService URL) is set in AddBusinessLayer.
    /// </summary>
    public class MessageClient : IMessageClient
    {
        private readonly HttpClient _httpClient;

        public MessageClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            HttpResponseMessage response;

            try
            {
                response = await _httpClient.PostAsJsonAsync("api/message/send-email", new { to, subject, body });
            }
            catch (HttpRequestException)
            {
                // MessageService isn't running or can't be reached.
                throw new ServiceUnavailableException("Email service is unavailable. Please try again later.");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new ServiceUnavailableException("Could not send the email. Please try again later.");
            }
        }
    }
}
