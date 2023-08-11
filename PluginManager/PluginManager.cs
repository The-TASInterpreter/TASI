using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Reflection;

namespace TASI.PluginManager
{
    internal class PluginManager
    {
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

        public static void InitialisePlugins(IEnumerable<ITASIPlugin> plugins, Global global)
        {
            foreach (ITASIPlugin plugin in plugins)
            {
                if (plugin is IInternalFunctionPlugin internalFunctionPlugin)
                {
                    internalFunctionPlugin.InitFunctions(global);
                }
            }
        }

    }
}
