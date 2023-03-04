using Cosmos.Cms.Common.Models;
using Cosmos.Cms.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cosmos.Cms.Models
{
    /// <summary>
    /// CKEditor post model
    /// </summary>
    public class HtmlEditorPost
    {
        /// <summary>
        /// Article ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Article number
        /// </summary>
        public int ArticleNumber { get; set; }

        /// <summary>
        ///     Article title
        /// </summary>
        [MaxLength(80)]
        [StringLength(80)]
        [Display(Name = "Article title")]
        [ArticleTitleValidation]
        [Remote("CheckTitle", "Edit", AdditionalFields = "ArticleNumber")]
        public string Title { get; set; }

        /// <summary>
        ///     Roles allowed to view this page.
        /// </summary>
        /// <remarks>If this value is null, it assumes page can be viewed anonymously.</remarks>
        public string RoleList { get; set; }

        /// <summary>
        ///     Date and time of when this was published
        /// </summary>
        [Display(Name = "Publish on date/time (PST):")]
        [DataType(DataType.DateTime)]
        [DateTimeUtcKind]
        public DateTimeOffset? Published { get; set; }

        /// <summary>
        /// Update existing article (don't create a new version)
        /// </summary>
        public bool? UpdateExisting { get; set; }

        /// <summary>
        ///     Date and time of when this was updated
        /// </summary>
        [Display(Name = "Updated on date/time (PST):")]
        [DataType(DataType.DateTime)]
        [DateTimeUtcKind]
        public DateTimeOffset? Updated { get; set; }

        /// <summary>
        /// Regions
        /// </summary>
        public List<HtmlEditorRegion> Regions { get; set; }

        #region ITEMS RETURNED AFTER SAVE (/Editor/PostRegions)

        /// <summary>
        /// URL path returned after save
        /// </summary>
        public string UrlPath { get; set; }
        
        /// <summary>
        /// Version number returned after save
        /// </summary>
        public int VersionNumber { get; set; }

        /// <summary>
        /// Content returned after save
        /// </summary>
        public string Content { get; set; }

        #endregion
    }

    /// <summary>
    /// CK Editor HTML Editable Region
    /// </summary>
    public class HtmlEditorRegion
    {
        /// <summary>
        /// Region ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// HTML Content
        /// </summary>
        public string Html { get; set; }
    }
}
