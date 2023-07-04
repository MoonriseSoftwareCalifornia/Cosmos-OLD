using System.Net;

namespace Cosmos.EmailServices
{
    public class SendResult
    {
        /// <summary>
        /// Gets or sets the status code returned from email handler.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets a value indicating whether Status Code of this response indicates success.
        /// </summary>
        public bool IsSuccessStatusCode
        {
            get
            {
                if (StatusCode >= HttpStatusCode.OK)
                {
                    return StatusCode <= (HttpStatusCode)299;
                }

                return false;
            }
        }

        /// <summary>
        /// Message returned by email system.
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
