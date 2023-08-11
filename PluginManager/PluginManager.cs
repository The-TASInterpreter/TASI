using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Reflection;

namespace TASI.PluginManager
{
    internal class PluginManager
    {
        /// <summary>
        /// The current supported plugin loader version of the plugin manager. Plugins on this version should have no problem to be loaded
        /// </summary>
        internal const int PLUGIN_COMPATIBILITY_VERSION = 1;

        /// <summary>
        /// The oldest supported plugin loader version of the plugin manager. Plugins between the current and oldest version should still load successfully
        /// Versions older than this might still work depending on the plugin type they're using
        /// </summary>
        internal const int OLDEST_SUPPORTED_PLUGIN_COMPATIBILITY_VERSION = 1;

        public static Assembly LoadPluginAssembly(string relativePath) // https://learn.microsoft.com/en-us/dotnet/core/tutorials/creating-app-with-plugin-support
        {
            // Navigate up to the solution root
            //Note from Ekischleki: I have no idea what that does
            string root = Path.GetFullPath(Path.Combine(
                Path.GetDirectoryName(
                    Path.GetDirectoryName(
                        Path.GetDirectoryName(
                            Path.GetDirectoryName(
                                Path.GetDirectoryName(typeof(Program).Assembly.Location)))))));
            Console.WriteLine(typeof(Program).Assembly.Location);

            string pluginLocation = Path.GetFullPath(Path.Combine(root, relativePath.Replace('\\', Path.DirectorySeparatorChar)));
            Console.WriteLine($"Loading commands from: {pluginLocation}");
            AssemblyLoader loadContext = new(pluginLocation);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }

        public static IEnumerable<ITASIPlugin> GetPluginsFromAssembly(Assembly assembly)
        {
            int found = 0;
            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(ITASIPlugin).IsAssignableFrom(type))
                {
                    ITASIPlugin? foundPlugin = Activator.CreateInstance(type) as ITASIPlugin;
                    if (foundPlugin != null)
                    {

                        found++;
                        yield return foundPlugin;
                    }
                }
            }

        }

        public static void InitialiseAndCheckPlugins(IEnumerable<ITASIPlugin> plugins, AccessableObjects accessableObjects)
        {
            foreach (ITASIPlugin plugin in plugins)
            {
                if (plugin.CompatibilityVersion > PLUGIN_COMPATIBILITY_VERSION)
                    throw new FaultyPluginException("The plugin compatibility version is greater that the plugin compatibility version of this program. Try to download the newest release and try again.", plugin);

                if (plugin is IInternalFunctionPlugin internalFunctionPlugin)
                {
                    internalFunctionPlugin.InitFunctions(accessableObjects.global);
                } else if (plugin is IInitialisationPlugin initialisationPlugin)
                {
                    initialisationPlugin.Execute(accessableObjects);
                } else
                {
                    throw new FaultyPluginException("This plugin type is not supported. Please keep in mind that the ITASIPlugin cannot be used directly as it just defines the base of a plugin.", plugin);
                }

                
            }
        }

    }
}
