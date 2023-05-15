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
        public string? DefaultFromEmailAddress { get; set; }
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
    }
}