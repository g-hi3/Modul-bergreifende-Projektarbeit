using System.Collections.Generic;
using System.Reflection;

namespace PluginSystem.Core
{
    public interface IPluginEnvironment
    {
        IEnumerable<Assembly> GetAssemblies();
    }
}