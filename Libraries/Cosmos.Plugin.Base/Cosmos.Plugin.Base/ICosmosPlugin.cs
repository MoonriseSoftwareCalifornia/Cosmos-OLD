using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.Cms.PluginBase
{
    /// <summary>
    /// Interface for Cosmos Plugins
    /// </summary>
    public interface ICosmosPlugin
    {
        #region PROPERTIES
        /// <summary>
        /// Author of plugin
        /// </summary>
        string Author { get; }
        /// <summary>
        /// Author website
        /// </summary>
        Uri AuthorUrl { get; }
        /// <summary>
        /// Description of what the plugin does
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Name of the plugin
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Version of plugin
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Paths to templates
        /// </summary>
        IEnumerable<string> TemplatePaths { get; }

        #endregion

        #region METHODS

        /// <summary>
        /// Configuration that gets loaded from settings
        /// </summary>
        /// <param name="config"></param>
        void Config(string config);

        /// <summary>
        /// Execution method
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        string Execute(string[] args);

        #endregion
    }
}
