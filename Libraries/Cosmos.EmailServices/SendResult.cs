namespace Cosmos.EmailServices
{
    public class SendResult
    {
        /// <summary>
        /// Send is successfull
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Message returned by email system.
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
