﻿// <copyright file="CatalogEntry.cs" company="Moonrise Software, LLC">
// Copyright (c) Moonrise Software, LLC. All rights reserved.
// Licensed under the GNU Public License, Version 3.0 (https://www.gnu.org/licenses/gpl-3.0.html)
// See https://github.com/MoonriseSoftwareCalifornia/CosmosCMS
// for more information concerning the license and the contributors participating to this project.
// </copyright>

namespace Cosmos.Common.Data
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Article list item.
    /// </summary>
    public class CatalogEntry
    {
        /// <summary>
        ///     Gets or sets article number.
        /// </summary>
        [Key]
        [Display(Name = "Article#")]
        public int ArticleNumber { get; set; }

        /// <summary>
        ///     Gets or sets title of the page, also used as the basis for the URL.
        /// </summary>
        [Display(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets disposition of the page.
        /// </summary>
        [Display(Name = "Status")]
        public string Status { get; set; }

        /// <summary>
        ///     Gets or sets date/time of when this page was last updated.
        /// </summary>
        [Display(Name = "Updated")]
        public DateTimeOffset Updated { get; set; }

        /// <summary>
        ///     Gets or sets date and time of when this item was published, and made public.
        /// </summary>
        [Display(Name = "Publish date/time")]
        public DateTimeOffset? Published { get; set; }

        /// <summary>
        ///     Gets or sets url of this item.
        /// </summary>
        [Display(Name = "Url")]
        public string UrlPath { get; set; }
    }
}
