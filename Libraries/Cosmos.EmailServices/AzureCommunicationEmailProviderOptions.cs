namespace Cosmos.EmailServices
{
    public class AzureCommunicationEmailProviderOptions
    {
        /// <summary>
        /// Connection string to the Azure communications email service.
        /// </summary>
        public required string ConnectionString { get; set; }
        /// <summary>
        /// Default 'from' email address (if not using the default).
        /// </summary>
        public string DefaultFromEmailAddress { get; set; } = "DoNotReply@notifications.cosmoswps.com";
    }
}
