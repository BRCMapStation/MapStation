#if BEPINEX
using BepInEx.Logging;
#endif
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapStation.Common.Runtime {
    public static class GamePluginManager {
#if BEPINEX
        private static ManualLogSource LogSource = Logger.CreateLogSource("MapStation Plugin Manager");
#endif

        private static List<AGameMapStationPlugin> GetPlugins() {
            var plugins = new List<AGameMapStationPlugin>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies) {
                var types = assembly.GetTypes();
                foreach (var type in types) {
                    try {
                        if (typeof(AGameMapStationPlugin).IsAssignableFrom(type) && !type.IsAbstract) {
                            var pluginInstance = Activator.CreateInstance(type) as AGameMapStationPlugin;
                            plugins.Add(pluginInstance);
                        }
                    }
                    catch(Exception e) {
#if BEPINEX
                        LogSource.LogWarning($"Problem loading a plugin, silently handled.{Environment.NewLine}{e}");
#else
                        UnityEngine.Debug.LogWarning($"Problem loading a plugin, silently handled.{Environment.NewLine}{e}");
#endif
                    }
                }
            }
            return plugins;
        }

        public static void OnAddMapToDatabase(ZipArchive archive, string path, string mapName) {
            var plugins = GetPlugins();
            foreach (var plugin in plugins) {
                plugin.OnAddMapToDatabase(archive, path, mapName);
            }
        }
    }
}
