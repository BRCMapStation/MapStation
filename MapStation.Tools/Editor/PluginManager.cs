using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapStation.Tools {
    public static class PluginManager {
        public static List<string> GetDependencies() {
            var dependencies = new List<string>();
            dependencies.Add(BuildConstants.ThunderstoreMapstationDependency);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies) {
                var types = assembly.GetTypes();
                foreach(var type in types) {
                    if (typeof(AMapStationPlugin).IsAssignableFrom(type) && !type.IsAbstract) {
                        var pluginInstance = Activator.CreateInstance(type) as AMapStationPlugin;
                        dependencies.AddRange(pluginInstance.GetDependencies());
                    }
                }
            }
            return dependencies;
        }
    }
}
