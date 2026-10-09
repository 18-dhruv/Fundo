using System.ComponentModel.DataAnnotations;

namespace MessageService.Models
{
    public class EmailRequestModel
    {
        [Required]
        [EmailAddress]
        public string To { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Body { get; set; } = string.Empty;
    }
}
