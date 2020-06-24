using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using PluginSystem.Core;

namespace PluginSystem.Environments
{
    public class FileSystemPluginEnvironment : IPluginEnvironment
    {
        private const string DefaultPluginDirectory = ".";
        private const SearchOption DefaultSearchOption = SearchOption.TopDirectoryOnly;
        private const string DllSearchPattern = "*.dll";

        private readonly string _pluginDirectory;
        private readonly SearchOption _searchOption;

        public FileSystemPluginEnvironment(string pluginDirectory = DefaultPluginDirectory,
            SearchOption searchOption = DefaultSearchOption)
        {
            _pluginDirectory = pluginDirectory;
            _searchOption = searchOption;
        }

        public IEnumerable<Assembly> GetAssemblies()
        {
            var filePaths = Directory.GetFiles(_pluginDirectory, DllSearchPattern, _searchOption);
            return filePaths.Select(Assembly.LoadFile)
                .ToList();
        }
    }
}