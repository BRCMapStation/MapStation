using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapStation.Tools {
    public static class PluginManager {
        public static List<string> GetDependencies() {
            var dependencies = new List<string>();
            dependencies.Add(BuildConstants.ThunderstoreMapstationDependency);
            var plugins = GetPlugins();
            foreach(var plugin in plugins) {
                dependencies.AddRange(plugin.GetDependencies());
            }
            return dependencies;
        }

        public static void ProcessThunderstoreZip(ZipArchive archive, string mapName) {
            var plugins = GetPlugins();
            foreach(var plugin in plugins) {
                plugin.ProcessThunderstoreZip(archive, mapName);
            }
        }

        private static List<AMapStationPlugin> GetPlugins() {
            var plugins = new List<AMapStationPlugin>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies) {
                var types = assembly.GetTypes();
                foreach (var type in types) {
                    if (typeof(AMapStationPlugin).IsAssignableFrom(type) && !type.IsAbstract) {
                        var pluginInstance = Activator.CreateInstance(type) as AMapStationPlugin;
                        plugins.Add(pluginInstance);
                    }
                }
            }
            return plugins;
        }
    }
}
