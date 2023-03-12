using Cosmos.Cms.PluginBase;

namespace Cosmos.Cms.Plugins.Hello
{
    /// <summary>
    /// Example plugin
    /// </summary>
    public class HelloPlugin : ICosmosPlugin
    {
        /// <summary>
        /// Plugin author
        /// </summary>
        public string Author => "Moonrise Software LLC";
        /// <summary>
        /// Author URL
        /// </summary>
        public Uri AuthorUrl => new Uri("https://www.moonrise.net");
        /// <summary>
        /// Description
        /// </summary>
        public string Description => "This is an example plugin for Cosmos WPS";
        /// <summary>
        /// Name of this plugin
        /// </summary>
        public string Name => "Cosmos Hello!";
        /// <summary>
        /// Version of this plugin
        /// </summary>
        public string Version => "1.0.0.1";

        /// <summary>
        /// Loads the configuration
        /// </summary>
        /// <param name="config"></param>
        public void Config(string config)
        {
            // Nothing to do now.
        }

        /// <summary>
        /// Method to execute
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public string Execute(string[] args)
        {
            return "Hello Cosmos!";
        }
    }
}