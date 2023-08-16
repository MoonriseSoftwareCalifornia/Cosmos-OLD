using Cosmos.BlobService;
using Cosmos.Common.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cosmos.Common
{
    /// <summary>
    /// Static utilities class
    /// </summary>
    public static class CosmosUtilities
    {
        /// <summary>
        /// Authenticates a user using article permissions
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="articleNumber"></param>
        /// <returns></returns>
        public static async Task<bool> AuthUser(ApplicationDbContext dbContext, ClaimsPrincipal user, int articleNumber)
        {
            List<ArticlePermission> permissions = null;
            try
            {
               permissions =  dbContext.Pages.LastOrDefault(l => l.ArticleNumber == articleNumber).ArticlePermissions;
            }
            catch (Exception ex)
            {
                var message = ex.Message;// Debugging
            }

            if (permissions == null || !permissions.Any()) { return true; }

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (permissions.Any(a => a.IdentityObjectId.Equals(userId, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            var objectIds = permissions.Where(w => w.IsRoleObject).Select(s => s.IdentityObjectId).ToArray();

            return (await dbContext.UserRoles.CountAsync(a => a.UserId == userId && objectIds.Contains(a.RoleId))) > 0;
        }

        /// <summary>
        /// Gets the folder contents for an article.
        /// </summary>
        /// <param name="_options"></param>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="articleNumber"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <remarks>Does NOT authenticate the user!</remarks>
        public static async Task<List<FileManagerEntry>> GetArticleFolderContents(StorageContext storageContext, int articleNumber, string path = "")
        {
            path = $"/pub/articles/{articleNumber}/{path.TrimStart('/')}";

            var contents = await storageContext.GetFolderContents(path);

            return contents;

        }
    }
}
