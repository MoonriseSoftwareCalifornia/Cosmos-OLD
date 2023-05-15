namespace Cosmos.EmailServices
{
    public class AzureCommunicationEmailProviderOptions
    {
        /// <summary>
        /// Connection string to the Azure communications email service.
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// Default 'from' email address if none given at send time.
        /// </summary>
        public string? DefaultFromEmailAddress { get; set; }
    }
}
