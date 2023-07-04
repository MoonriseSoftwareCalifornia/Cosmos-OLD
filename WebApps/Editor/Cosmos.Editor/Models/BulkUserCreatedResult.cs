using Cosmos.EmailServices;
using Microsoft.AspNetCore.Identity;

namespace Cosmos.Cms.Models
{
    /// <summary>
    /// Returns the result of creating a user with the bulk-create method.
    /// </summary>
    public class BulkUserCreatedResult
    {
        /// <inheritdoc/>
        public IdentityResult IdentityResult { get; set; }

        /// <inheritdoc/>
        public SendResult SendResult { get; set; }

        /// <inheritdoc/>
        public UserCreateViewModel UserCreateViewModel { get; internal set; }
    }
}
