using System;
using System.Collections;
using PluginSystem.Api;

namespace PluginSystem.Core.Test
{
    public class TestPluginExporter : IDataExportPlugin
    {
        public void Export(IEnumerable data, string exportPath)
        {
            throw new NotImplementedException();
        }

        public string Name { get; }
    }
}