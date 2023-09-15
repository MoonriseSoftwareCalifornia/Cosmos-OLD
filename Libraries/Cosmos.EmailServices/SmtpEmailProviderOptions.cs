namespace Cosmos.EmailServices
{
    /// <summary>
    /// SMTP Email client properties
    /// </summary>
    public class SmtpEmailProviderOptions
    {
        /// <summary>
        /// From email address
        /// </summary>
        public string DefaultFromEmailAddress { get; set; } = "donotreply@cosmosws.io";
        /// <summary>
        /// SMTP Host name
        /// </summary>
        public string? Host { get; set; }
        /// <summary>
        /// Account user name
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// SMTP Host password
        /// </summary>
        public string? Password { get; set; }
        /// <summary>
        /// SMTP Port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Use SSL for communications?
        /// </summary>
        /// <remarks>Default is false because TLS is the default mode.</remarks>
        public bool UsesSsl { get; set; } = false; // Uses TLS by default
    }
}