using MessageService.Models;
using MessageService.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MessageService.Controllers
{
    [ApiController]
    [Route("api/message")]
    public class MessageController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<MessageController> _logger;

        public MessageController(IEmailService emailService, ILogger<MessageController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail(EmailRequestModel request)
        {
            try
            {
                await _emailService.SendAsync(request.To, request.Subject, request.Body);
                return Ok(new { isSuccess = true, message = "Email sent." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To}", request.To);
                return StatusCode(StatusCodes.Status502BadGateway, new { isSuccess = false, message = "Failed to send email." });
            }
        }
    }
}
