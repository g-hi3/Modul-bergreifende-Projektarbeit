using System.Collections;

namespace PluginSystem.Api
{
    public interface IDataExportPlugin
    {
        string Name { get; }
        void Export(IEnumerable data, string exportPath);
    }
}