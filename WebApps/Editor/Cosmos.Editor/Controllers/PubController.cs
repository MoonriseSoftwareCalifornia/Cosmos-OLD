using Cosmos.BlobService;
using Cosmos.Cms.Common.Services.Configurations;
using Cosmos.Common;
using Cosmos.Common.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Cosmos.Publisher.Controllers
{
    /// <summary>
    /// Secure file access controller and proxy
    /// </summary>
    public class PubController : Controller
    {
        private readonly IOptions<CosmosConfig> _options;
        private readonly ApplicationDbContext _dbContext;
        private readonly StorageContext _storageContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        /// <param name="dbContext"></param>
        /// <param name="storageContext"></param>
        public PubController(IOptions<CosmosConfig> options, ApplicationDbContext dbContext, StorageContext storageContext)
        {
            _options = options;
            _dbContext = dbContext;
            _storageContext = storageContext;
        }

        /// <summary>
        /// Gets a file and validates user authentication
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {

            if (_options.Value.SiteSettings.PublisherRequiresAuthentication)
            {
                // If the user is not logged in, have them login first.
                if (User.Identity == null || User.Identity?.IsAuthenticated == false)
                {
                    return Redirect("~/Identity/Account/Login?returnUrl=" + Request.Path);
                }

                if (User.IsInRole(_options.Value.SiteSettings.CosmosRequiredPublisherRole) == false)
                {
                    return Unauthorized();
                }

                var path = HttpContext.Request.Path.ToString().ToLower();

                if (path.StartsWith("/pub/articles/"))
                {
                    var id = path.TrimStart('/').Split('/')[2];

                    if (int.TryParse(id, out var articleNumber))
                    {

                        if (!await CosmosUtilities.AuthUser(_dbContext, User, articleNumber))
                        {
                            return Unauthorized();
                        }
                    }
                }

                Response.Headers.Expires = DateTimeOffset.UtcNow.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'");
            }

            var client = _storageContext.GetAppendBlobClient(HttpContext.Request.Path);
            var properties = await client.GetPropertiesAsync();

            return File(await client.OpenReadAsync(), properties.Value.ContentType);
        }

    }
}
