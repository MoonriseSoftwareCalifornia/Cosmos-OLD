using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Cosmos.EmailServices
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IOptions<SmtpEmailProviderOptions> _options;
        private readonly ILogger<SendGridEmailSender> _logger;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="options"></param>
        public SmtpEmailSender(IOptions<SmtpEmailProviderOptions> options, ILogger<SendGridEmailSender> logger)
        {
            _options = options;
            _logger = logger;
        }

        /// <summary>
        ///     Send email method
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(subject, message, email);
        }

        /// <summary>
        ///     Execute send email method
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="email"></param>
        /// <param name="emailFrom"></param>
        /// <returns></returns>
        private async Task Execute(string subject, string message, string email)
        {
            var client = new SmtpClient(_options.Value.Host, _options.Value.Port);

            if (!string.IsNullOrEmpty(_options.Value.Password))
            {
                client.Credentials = new NetworkCredential(_options.Value.UserName, _options.Value.Password);
                if (_options.Value.UsesSsl)
                {
                    client.EnableSsl = true;
                }
            }

            var msg = new MailMessage(_options.Value.DefaultFromEmailAddress, email, subject, message);
            
            try
            {
                await client.SendMailAsync(msg);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }


            return;
        }
    }

}
