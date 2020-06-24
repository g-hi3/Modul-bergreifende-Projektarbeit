using System.Collections.Generic;
using System.Reflection;

namespace PluginSystem.Core.Test
{
    internal class TestPluginEnvironment : IPluginEnvironment
    {
        private readonly IEnumerable<Assembly> Assemblies;

        public TestPluginEnvironment(IEnumerable<Assembly> assemblies)
        {
            Assemblies = assemblies;
        }

        public IEnumerable<Assembly> GetAssemblies()
        {
            return Assemblies;
        }
    }
}