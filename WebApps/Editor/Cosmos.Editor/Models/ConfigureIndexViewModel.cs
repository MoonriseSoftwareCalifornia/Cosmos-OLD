using Cosmos.Cms.Common.Services.Configurations;
using System.ComponentModel.DataAnnotations;

namespace Cosmos.Cms.Models
{
    /// <summary>
    ///     Configuration index view model
    /// </summary>
    public class ConfigureIndexViewModel : CosmosConfig
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public ConfigureIndexViewModel()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="secretName"></param>
        public ConfigureIndexViewModel(string secretName)
        {
            SecretName = secretName;
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="config"></param>
        public ConfigureIndexViewModel(CosmosConfig config)
        {
            if (config != null)
                Init(config);
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="secretName"></param>
        /// <param name="config"></param>
        public ConfigureIndexViewModel(string secretName, CosmosConfig config)
        {
            SecretName = secretName;
            if (config != null)
                Init(config);
        }

        /// <summary>
        /// Default Microsoft client ID (comes from boot time environment variables)
        /// </summary>
        public string DefaultMicrosoftClientId { get; set; }

        /// <summary>
        /// Default Microsoft secret (comes from boot time environment variables)
        /// </summary>
        public string DefaultMicrosoftSecret { get; set; }

        /// <summary>
        ///     This is the object used to load and deserialize a json object
        /// </summary>
        [Display(Name = "Import JSON")]
        public string ImportJson { get; set; }

        /// <summary>
        ///     AWS storage connections in JSON format
        /// </summary>
        public string AwsS3ConnectionsJson { get; set; }

        /// <summary>
        ///     Azure storage connections in JSON format
        /// </summary>
        public string AzureBlobConnectionsJson { get; set; }

        /// <summary>
        ///     Blob connection information in JSON format
        /// </summary>
        public string BlobConnectionsJson { get; set; }

        /// <summary>
        ///     Google storage connections in JSON format
        /// </summary>
        public string GoogleBlobConnectionsJson { get; set; }

        /// <summary>
        ///     Redis connections in JSON format
        /// </summary>
        public string RedisConnectionsJson { get; set; }

        /// <summary>
        ///     Database connection information in JSON format
        /// </summary>
        public string SqlConnectionsJson { get; set; }

        /// <summary>
        ///     List of editor URLs in JSON format
        /// </summary>
        public string EditorUrlsJson { get; set; }

        /// <summary>
        ///     Connection test success or not
        /// </summary>
        public bool TestSuccess { get; set; }

        /// <summary>
        /// Can save to secrets if secrets manager is configured
        /// </summary>
        public bool CanSaveSecrets { get; private set; }

        /// <summary>
        ///     Initializes the model
        /// </summary>
        /// <param name="config"></param>
        private void Init(CosmosConfig config)
        {
            SiteSettings = config.SiteSettings;
            ImportJson = string.Empty;
            CdnConfig = config.CdnConfig;
            SendGridConfig = config.SendGridConfig;
            StorageConfig = config.StorageConfig;
            base.SecretName = config.SecretName;

            EditorUrls = config.EditorUrls;
            CanSaveSecrets = config.SiteSettings.AllowConfigEdit;
            SecretName = config.SecretName;
        }

        /// <summary>
        ///     Gets the Cosmos Config
        /// </summary>
        /// <returns></returns>
        public CosmosConfig GetConfig()
        {
            return new()
            {
                CdnConfig = CdnConfig,
                GoogleCloudAuthConfig = GoogleCloudAuthConfig,
                SendGridConfig = SendGridConfig,
                SiteSettings = SiteSettings,
                StorageConfig = StorageConfig
            };
        }
    }
}