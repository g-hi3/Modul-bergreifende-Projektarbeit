using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moq;
using NUnit.Framework;
using PluginSystem.Api;

namespace PluginSystem.Core.Test
{
    public class PluginLoaderTest
    {
        [Test]
        public void LoadExporters_NoAssemblies_ExportersIsEmpty()
        {
            // Arrange
            var assemblies = new Assembly[0];
            var pluginEnvironment = new TestPluginEnvironment(assemblies);

            // Act
            var pluginLoader = new PluginLoader(pluginEnvironment);
            var exporters = pluginLoader.LoadExporters();

            // Assert
            Assert.That(exporters, Is.Empty);
        }

        [Test]
        public void LoadExporters_OneAssemblyOneLoader_ExportersHasLoader()
        {
            // Arrange
            var assemblyMock = FakeAssembly<TestPluginExporter>();
            var assemblies = new[] {assemblyMock.Object};
            var pluginEnvironment = new TestPluginEnvironment(assemblies);

            // Act
            var pluginLoader = new PluginLoader(pluginEnvironment);
            var exporters = pluginLoader.LoadExporters();

            // Assert
            var dataExportPlugins = exporters as IDataExportPlugin[] ?? exporters.ToArray();
            Assert.That(dataExportPlugins, Is.Not.Empty);
            Assert.True(dataExportPlugins.First() is TestPluginExporter);
        }

        [Test]
        public void LoadExporters_CallsGetAssembliesOnEnvironment()
        {
            // Arrange
            var pluginEnvironment = new Mock<IPluginEnvironment>();
            pluginEnvironment.Setup(environment => environment.GetAssemblies()).Returns(Enumerable.Empty<Assembly>());

            // Act
            var pluginLoader = new PluginLoader(pluginEnvironment.Object);
            pluginLoader.LoadExporters();

            // Assert
            pluginEnvironment.Verify(environment => environment.GetAssemblies(), Times.Once);
        }

        private static Mock<Assembly> FakeAssembly<T>()
            where T : IDataExportPlugin, new()
        {
            var mock = new Mock<Assembly>();
            mock.Setup(assembly => assembly.ExportedTypes).Returns(new List<Type> {typeof(T)});
            return mock;
        }
    }
}