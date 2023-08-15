using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using TASI.PluginManager;
using TASI.InterpretStartup;

namespace PluginLoader
{
    public class PluginLoader
    {
        public static IEnumerable<ITASIPlugin> LoadPlugins(string filePath, params Assembly[] additionalAssemblies)
        {
            var pluginLoadContext = new PluginLoadContext(filePath, additionalAssemblies);
            var assembly = pluginLoadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(filePath)));

            var pluginType = typeof(ITASIPlugin);
            var pluginTypes = new List<Type>();
            if (assembly != null)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsInterface || type.IsAbstract)
                    {
                        continue;
                    }
                    else
                    {
                        if (type.GetInterface(pluginType.FullName) != null)
                        {
                            pluginTypes.Add(type);
                        }
                    }
                }
            }

            var plugins = new List<ITASIPlugin>(pluginTypes.Count);
            foreach (var type in pluginTypes)
            {
                var plugin = (ITASIPlugin)Activator.CreateInstance(type);
                plugins.Add(plugin);
            }

            return plugins;
        }
    }

    public class PluginLoadContext : AssemblyLoadContext
    {
        private AssemblyDependencyResolver _resolver;
        private Dictionary<string, Assembly> _additionalAssemblies;

        public PluginLoadContext(string pluginPath, params Assembly[] additionalAssemblies) : base(isCollectible: true)
        {
            _resolver = new AssemblyDependencyResolver(pluginPath);
            this.Resolving += PluginLoadContext_Resolving;

            _additionalAssemblies = new Dictionary<string, Assembly>();
            foreach (var additionalAssembly in additionalAssemblies)
            {
                _additionalAssemblies[additionalAssembly.GetName().Name] = additionalAssembly;
            }
        }

        private Assembly PluginLoadContext_Resolving(AssemblyLoadContext arg1, AssemblyName arg2)
        {
            Console.WriteLine($"Resolving assembly: {arg2.FullName}");

            string assemblyPath = _resolver.ResolveAssemblyToPath(arg2);
            if (assemblyPath != null)
            {
                Console.WriteLine($"Loading assembly from path: {assemblyPath}");
                return LoadFromAssemblyPath(assemblyPath);
            }

            if (_additionalAssemblies.ContainsKey(arg2.Name))
            {
                Console.WriteLine($"Loading additional assembly: {arg2.FullName}");
                return _additionalAssemblies[arg2.Name];
            }

            Console.WriteLine($"Failed to resolve assembly: {arg2.FullName}");
            return null;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            Console.WriteLine($"Loading assembly: {assemblyName.FullName}");

            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                Console.WriteLine($"Loading assembly from path: {assemblyPath}");
                return LoadFromAssemblyPath(assemblyPath);
            }

            if (_additionalAssemblies.ContainsKey(assemblyName.Name))
            {
                Console.WriteLine($"Loading additional assembly: {assemblyName.FullName}");
                return _additionalAssemblies[assemblyName.Name];
            }

            if (assemblyName.Name == typeof(TASI_Main).Assembly.GetName().Name)
            {
                Console.WriteLine($"Loading TASI_Main assembly: {assemblyName.FullName}");
                return typeof(TASI_Main).Assembly;
            }

            Console.WriteLine($"Failed to load assembly: {assemblyName.FullName}");
            return null;
        }
    }
}
