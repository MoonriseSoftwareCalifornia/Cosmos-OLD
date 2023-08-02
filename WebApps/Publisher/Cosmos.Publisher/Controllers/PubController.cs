using Cosmos.BlobService;
using Cosmos.Cms.Common.Services.Configurations;
using Cosmos.Common.Data;
using Cosmos.Common.Data.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Cosmos.Publisher.Controllers
{
    public class PubController : Controller
    {
        private readonly ILogger<PubController> _logger;
        private readonly ArticleLogic _articleLogic;
        private readonly IOptions<CosmosConfig> _options;
        private readonly ApplicationDbContext _dbContext;
        private readonly StorageContext _storageContext;

        public PubController(ILogger<PubController> logger, ArticleLogic articleLogic, IOptions<CosmosConfig> options, ApplicationDbContext dbContext, StorageContext storageContext)
        {
            _logger = logger;
            _articleLogic = articleLogic;
            _options = options;
            _dbContext = dbContext;
            _storageContext = storageContext;
        }
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
                        var page = _dbContext.Pages.LastOrDefault(l => l.ArticleNumber == articleNumber);


                        if (page != null && page.ArticlePermissions.Any() && (await AuthenticateUser(page.ArticlePermissions)) == false)
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

        private async Task<bool> AuthenticateUser(int articleNumber)
        {
            var article = await _dbContext.Pages.OrderByDescending(o => o.Published).FirstOrDefaultAsync(f => f.ArticleNumber == articleNumber && f.Published != null && f.Published <= DateTimeOffset.UtcNow);
            if (article == null) return false;

            return await AuthenticateUser(article.ArticlePermissions);
        }

        private async Task<bool> AuthenticateUser(List<ArticlePermission> permissions)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (permissions.Any(a => a.IdentityObjectId.Equals(userId, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            var objectIds = permissions.Where(w => w.IsRoleObject).Select(s => s.IdentityObjectId).ToArray();

            return (await _dbContext.UserRoles.CountAsync(a => a.UserId == userId && objectIds.Contains(a.RoleId))) > 0;

        }
    }
}
