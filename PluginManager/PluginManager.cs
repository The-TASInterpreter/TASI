using System.Reflection;
using TASI.InterpretStartup;

namespace TASI.PluginManager
{
    internal class PluginManager
    {
        /// <summary>
        /// The current supported plugin loader version of the plugin manager. Plugins on this version should have no problem to be loaded
        /// </summary>
        internal const int PLUGIN_COMPATIBILITY_VERSION = 3;

        /// <summary>
        /// The oldest supported plugin loader version of the plugin manager. Plugins between the current and oldest version should still load successfully
        /// Versions older than this might still work depending on the plugin type they're using
        /// </summary>
        internal const int OLDEST_SUPPORTED_PLUGIN_COMPATIBILITY_VERSION = 3;

        public static Assembly LoadPluginAssembly(string relativePath) // https://learn.microsoft.com/en-us/dotnet/core/tutorials/creating-app-with-plugin-support
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                Console.WriteLine("Resorted to fallback host injector");
                // Get the Name of the assembly that failed to load
                string assemblyName = new AssemblyName(args.Name).Name;
                Console.WriteLine(assemblyName + " assembly Name");

                // Check if the failed assembly is one of the dependencies
                if (assemblyName == "TASI")
                {
                    // Get a reference to the currently executing assembly
                    Assembly hostAssembly = Assembly.GetExecutingAssembly();

                    // Return the host assembly
                    return hostAssembly;
                }

                return null;
            };
            return Assembly.LoadFile(relativePath);


            // Navigate up to the solution root
            //Note from Ekischleki: I have no idea what that does
            string root =
                Path.GetDirectoryName(
                    Path.GetDirectoryName(
                        Path.GetDirectoryName(
                            Path.GetDirectoryName(
                                Path.GetDirectoryName(
                                    typeof(TASI_Main).Assembly.Location)))));
            Console.WriteLine(Path.Combine(root, relativePath.Replace('\\', Path.DirectorySeparatorChar)));
            Console.WriteLine(root);
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



        public static void InitFunctionPlugins(IEnumerable<ITASIPlugin> plugins, Global global)
        {
            foreach (ITASIPlugin plugin in plugins)
            {
                if (plugin is IInternalFunctionPlugin functionPlugin)
                {
                    functionPlugin.InitFunctions(global);
                }
            }
        }
        public static void InitBeforeRuntimePlugins(IEnumerable<ITASIPlugin> plugins, AccessableObjects accessableObjects)
        {
            foreach (ITASIPlugin plugin in plugins)
            {
                if (plugin is IInitialisationPlugin initPlugin)
                {
                    initPlugin.Execute(accessableObjects);
                }
                if (plugin is IStatementPlugin statementPlugin)
                {
                    statementPlugin.InitStatements(accessableObjects.global);
                }
            }
        }

        public static void CheckPlugins(IEnumerable<ITASIPlugin> plugins)
        {
            foreach (ITASIPlugin plugin in plugins)
            {
                if (plugin.CompatibilityVersion > PLUGIN_COMPATIBILITY_VERSION)
                    throw new FaultyPluginException("The plugin compatibility version is greater that the plugin compatibility version of this program. Try to download the newest release and try again.", plugin);

                switch (plugin)
                {
                    case IInitialisationPlugin:
                    case IInternalFunctionPlugin:
                    case IStatementPlugin:
                        break;
                    default:
                        throw new FaultyPluginException("This plugin type is not supported. Please keep in mind that the ITASIPlugin cannot be used directly as it just defines the base of a plugin.", plugin);

                }



            }
        }

    }
}
