namespace Cosmos.Common.Data
{
    /// <summary>
    /// Article permission for a role or user
    /// </summary>
    public class ArticlePermission
    {
        /// <summary>
        /// Role or user ID
        /// </summary>
        public string IdentityObjectId {  get; set; }
        /// <summary>
        /// Permission (Read or Upload)
        /// </summary>
        public string Permission { get; set; } = "Read";

        public bool IsRoleObject { get; set; } = true;
    }
}
