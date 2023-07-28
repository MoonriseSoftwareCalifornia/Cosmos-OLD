namespace Cosmos.Editor.Models
{
    /// <summary>
    /// Azure Frontdoor connection information
    /// </summary>
    public class FrontdoorConnection
    {
        /// <summary>
        /// All values are configured
        /// </summary>
        /// <returns></returns>
        public bool IsConfigured ()
        {
            return !string.IsNullOrEmpty(SubscriptionId)
                && !string.IsNullOrEmpty(ResourceGroupName)
                && !string.IsNullOrEmpty(FrontDoorName)
                && !string.IsNullOrEmpty(EndpointName)
                && !string.IsNullOrEmpty(TenantId)
                && !string.IsNullOrEmpty(ClientId)
                && !string.IsNullOrEmpty(ClientSecret)
                && !string.IsNullOrEmpty(DnsNames);
        }

        /// <summary>
        /// Subscription ID of where FD is located
        /// </summary>
        public string SubscriptionId { get; set; }
        /// <summary>
        /// Resource group name where FD is located
        /// </summary>
        public string ResourceGroupName { get; set; }
        /// <summary>
        /// Frontdoor name
        /// </summary>
        public string FrontDoorName { get; set; }
        /// <summary>
        /// Front door endpoint name (specific to a website)
        /// </summary>
        public string EndpointName { get; set; }
        /// <summary>
        /// Tenent ID of where FD is located
        /// </summary>
        public string TenantId { get; set; }
        /// <summary>
        /// Registered application ID (client ID) that has access to this FD
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// Client secret of the application ID (client ID) that has access to this FD
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Comma delimited list of DNS names to purge
        /// </summary>
        public string DnsNames {  get; set; }
    }
}
