using Cosmos.Common.Data;
using Cosmos.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cosmos.Editor.Models
{
    /// <summary>
    /// Article permissions view model
    /// </summary>
    public class ArticlePermissionsViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ArticlePermissionsViewModel()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="article"></param>
        /// <param name="forRoles"></param>
        public ArticlePermissionsViewModel(ArticleViewModel article, bool forRoles = true)
        {
            Id = article.Id;
            Title = article.Title;
            Published = article.Published;
            ShowingRoles = forRoles;
        }
        /// <summary>
        /// Specific article version we are setting the permissions for.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Article number
        /// </summary>
        public int ArticleNumber { get; set; }
        /// <summary>
        /// Article title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Date and time published
        /// </summary>
        public DateTimeOffset? Published { get; set; }

        /// <summary>
        /// Permission set is for roles, otherwise is for users
        /// </summary>
        public bool ShowingRoles { get; set; }

    }
}
