namespace Cosmos.Editor.Models
{
    /// <summary>
    /// Article permission item
    /// </summary>
    public class ArticlePermisionItem
    {
        /// <summary>
        /// Role or user ID
        /// </summary>
        public string IdentityObjectId { get; set; }

        /// <summary>
        /// Role name or user email
        /// </summary>
        public string Name { get; set; }

    }

}
