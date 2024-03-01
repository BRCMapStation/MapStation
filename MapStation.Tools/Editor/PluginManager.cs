#if BEPINEX
using BepInEx.Logging;
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace MapStation.Tools {
    public static class PluginManager {
#if BEPINEX
        private static ManualLogSource LogSource = Logger.CreateLogSource("MapStation Plugin Manager");
#endif

        public static List<string> GetDependencies() {
            var dependencies = new List<string>();
            var plugins = GetPlugins();
            foreach(var plugin in plugins) {
                dependencies.AddRange(plugin.GetDependencies());
            }
            return dependencies;
        }

        public static void ProcessMapZip(ZipArchive archive, string mapName, CompressionLevel compressionLevel) {
            var plugins = GetPlugins();
            foreach (var plugin in plugins) {
                plugin.ProcessMapZip(archive, mapName, compressionLevel);
            }
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
                    try {
                        if (typeof(AMapStationPlugin).IsAssignableFrom(type) && !type.IsAbstract) {
                            var pluginInstance = Activator.CreateInstance(type) as AMapStationPlugin;
                            plugins.Add(pluginInstance);
                        }
                    }
                    catch(Exception e) {
#if BEPINEX
                        LogSource.LogWarning($"Problem loading a MapStation plugin, silently handled.{Environment.NewLine}{e}");
#else
                        UnityEngine.Debug.LogWarning($"Problem loading a MapStation plugin, silently handled.{Environment.NewLine}{e}");
#endif
                    }
                }
            }
            return plugins;
        }
    }
}
