using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI.PluginManager
{
    /// <summary>
    /// The base interface for all TASI plugins. This interface can't be used alone
    /// </summary>
    public interface ITASIPlugin
    {
        /// <summary>
        /// The Name of the plugin
        /// </summary>
        string Name { get; } 

        /// <summary>
        /// The internal plugin version
        /// </summary>
        string Version { get; }

        /// <summary>
        /// The plugin description
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The host plugin loader version the plugin was written for (You can find it here: <see cref="PluginManager.PLUGIN_COMPATIBILITY_VERSION"/>)
        /// </summary>
        int CompatibilityVersion { get; }

        /// <summary>
        /// The plugin copyright
        /// </summary>
        string Copyright { get; }

        /// <summary>
        /// The author/authors of the plugin
        /// </summary>
        string Author { get; }


    }
}
