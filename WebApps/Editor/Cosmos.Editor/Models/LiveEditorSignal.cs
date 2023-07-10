using System;

namespace Cosmos.Cms.Models
{
    /// <summary>
    /// Live editor SignalR model
    /// </summary>
    public class LiveEditorSignal
    {
        /// <summary>
        /// Article record ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Id of the Article entity being worked on.
        /// </summary>
        public int ArticleNumber { get; set; }

        /// <summary>
        /// Edit ID as defined by the data-ccms-ceid attribute.
        /// </summary>
        public string EditorId { get; set; }

        /// <summary>
        /// User Id (Email address)
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// User position in document
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Command
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Is Focused
        /// </summary>
        public bool IsFocused { get; set; }

        /// <summary>
        /// Page version number
        /// </summary>
        public int VersionNumber { get; set; }

        /// <summary>
        /// HTML data being sent back
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// Date/time published
        /// </summary>
        public DateTimeOffset? Published { get; set; }
        /// <summary>
        /// Date/time updated
        /// </summary>
        public DateTimeOffset? Updated { get; set; }
        /// <summary>
        /// Article title
        /// </summary>
        public string Title { get; set; } = "";
        /// <summary>
        /// URL path
        /// </summary>
        public string UrlPath { get; set; } = "";
        public string BannerImage { get; internal set; }
        public string RoleList { get; internal set; }
    }
}
