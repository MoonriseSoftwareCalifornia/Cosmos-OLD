using Azure.Communication.Email;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Cosmos.EmailServices
{
    /// <summary>
    /// Cosos Email Sender Interface
    /// </summary>
    /// <remarks>Includes the result property</remarks>
    public interface ICosmosEmailSender : IEmailSender
    {
        EmailSendStatus SendStatus { get; set; }
    }
}
