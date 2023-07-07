using System.ComponentModel.DataAnnotations;

namespace Cosmos.Common.Models
{
    /// <summary>
    /// Email message view model
    /// </summary>
    public class EmailMessageViewModel
    {
        /// <summary>
        /// Sender name
        /// </summary>
        [Display(Name = "Your name:")]
        [Required(AllowEmptyStrings = false)]
        public string SenderName { get; set; }
        /// <summary>
        /// Email address
        /// </summary>
        [EmailAddress]
        [MaxLength(156)]
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Your email address (required/will not be shared):")]
        public string FromEmail { get; set; }

        /// <summary>
        /// Email subject
        /// </summary>
        [MaxLength(256)]
        [Display(Name = "Subject (optional):")]
        public string Subject { get; set; }

        /// <summary>
        /// Email content
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [MaxLength(2048)]
        public string Content { get; set; }

        public bool? SendSuccess { get; set; }
    }
}
