using System;
using System.Collections;
using PluginSystem.Api;

namespace PluginSystem.Exporters
{
    public class JsonDataExporter : IDataExportPlugin
    {
        public void Export(IEnumerable data, string exportPath)
        {
            throw new NotImplementedException();
        }

        public string Name => "JSON Data Exporter";
    }
}