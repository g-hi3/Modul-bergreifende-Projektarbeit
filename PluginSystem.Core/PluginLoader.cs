using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PluginSystem.Api;

namespace PluginSystem.Core
{
    public class PluginLoader
    {
        private static readonly Type ExporterClass = typeof(IDataExportPlugin);

        public PluginLoader(IPluginEnvironment environment)
        {
            Environment = environment;
        }

        private IPluginEnvironment Environment { get; }

        public IEnumerable<IDataExportPlugin> LoadExporters()
        {
            var loadedAssemblies = LoadAssemblies();
            var implementingClasses = GetAllImplementingClasses(loadedAssemblies);
            var exporters = Instantiate(implementingClasses);
            return exporters;
        }

        private IEnumerable<Assembly> LoadAssemblies()
        {
            return Environment.GetAssemblies();
        }

        private static IEnumerable<Type> GetAllImplementingClasses(IEnumerable<Assembly> assemblies)
        {
            var allImplementingClasses = new List<Type>();
            foreach (var nextAssembly in assemblies)
            {
                var implementingClasses = GetClasses(nextAssembly);
                allImplementingClasses.AddRange(implementingClasses);
            }

            return allImplementingClasses;
        }

        private static IEnumerable<Type> GetClasses(Assembly assembly)
        {
            return assembly.ExportedTypes
                .Where(nextType => ExporterClass.IsAssignableFrom(nextType))
                .ToList();
        }

        private static IEnumerable<IDataExportPlugin> Instantiate(IEnumerable<Type> classes)
        {
            return classes.Select(nextClass => (IDataExportPlugin) Activator.CreateInstance(nextClass))
                .ToList();
        }
    }
}