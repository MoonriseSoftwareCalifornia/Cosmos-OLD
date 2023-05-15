using Azure.Communication.Email;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cosmos.EmailServices
{
    public class AzureCommunicationEmailSender : IEmailSender
    {
        private readonly IOptions<AzureCommunicationEmailProviderOptions> _options;
        private readonly ILogger<AzureCommunicationEmailSender> _logger;

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

                if (result.Value.Status == EmailSendStatus.Succeeded)
                {
                    _logger.LogInformation($"Email successfully sent to: {email}; Subject: {subject};");
                }
                else if (result.Value.Status == EmailSendStatus.Failed)
                {
                    var response = result.GetRawResponse();
                    _logger.LogError($"Email FAILED attempting to send to: {email}, with subject: {subject}, with error: {response.ReasonPhrase}");
                }
                else if (result.Value.Status == EmailSendStatus.Canceled)
                {
                    var response = result.GetRawResponse();
                    _logger.LogInformation($"Email was CANCELED to: {email}, with subject: {subject}, with reason: {response.ReasonPhrase}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }

        }
    }
}
