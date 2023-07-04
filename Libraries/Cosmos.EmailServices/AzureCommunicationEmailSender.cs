using Azure.Communication.Email;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Cosmos.EmailServices
{
    public class AzureCommunicationEmailSender : IEmailSender
    {
        private readonly IOptions<AzureCommunicationEmailProviderOptions> _options;
        private readonly ILogger<AzureCommunicationEmailSender> _logger;

        /// <summary>
        /// Result of the last email send
        /// </summary>
        public SendResult? SendResult { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public AzureCommunicationEmailSender(IOptions<AzureCommunicationEmailProviderOptions> options, ILogger<AzureCommunicationEmailSender> logger)
        {
            _options = options;
            _logger = logger;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailClient = new EmailClient(_options.Value.ConnectionString);

            try
            {

                var result = await emailClient.SendAsync(Azure.WaitUntil.Completed, _options.Value.DefaultFromEmailAddress, email, subject, htmlMessage);

                var response = result.GetRawResponse();

                SendResult = new SendResult();
                SendResult.StatusCode = (HttpStatusCode) response.Status;
                

                if (result.Value.Status == EmailSendStatus.Succeeded)
                {
                    SendResult.Message = $"Email successfully sent to: {email}; Subject: {subject};";

                }
                else if (result.Value.Status == EmailSendStatus.Failed)
                {
                    SendResult.Message = $"Email FAILED attempting to send to: {email}, with subject: {subject}, with error: {response.ReasonPhrase}";
                }
                else if (result.Value.Status == EmailSendStatus.Canceled)
                {
                    SendResult.Message = $"Email was CANCELED to: {email}, with subject: {subject}, with reason: {response.ReasonPhrase}";
                }

                _logger.LogInformation(SendResult.Message);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }

        }
    }
}
