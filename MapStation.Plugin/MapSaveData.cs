using CommonAPI;
using MapStation.Common.Runtime;
using Reptile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapStation.Plugin {
    public class MapSaveData : CustomSaveData {
        public static MapSaveData Instance;
        private const byte Version = 1;
        public Dictionary<string, LoadedMapOptions> Options = new();

        public MapSaveData() : base(PluginInfo.PLUGIN_GUID, "{0}.cmsp") {

        }

        public override void Write(BinaryWriter writer) {
            writer.Write(Version);
            writer.Write(Options.Count);
            foreach(var loadedMapOptions in Options) {
                writer.Write(loadedMapOptions.Key);
                writer.Write(loadedMapOptions.Value.Options.Count);
                foreach(var mapOption in loadedMapOptions.Value.Options) {
                    writer.Write(mapOption.Key);
                    writer.Write(mapOption.Value);
                }
            }
        }

        public override void Read(BinaryReader reader) {
            var version = reader.ReadByte();
            var mapOptionCount = reader.ReadInt32();
            for(var i = 0; i < mapOptionCount; i++) {
                var loadedMapOptions = new LoadedMapOptions();
                var key = reader.ReadString();
                Options[key] = loadedMapOptions;
                var optionCount = reader.ReadInt32();
                for(var n = 0; n < optionCount; n++) {
                    var optionKey = reader.ReadString();
                    var optionValue = reader.ReadString();
                    loadedMapOptions.Options[optionKey] = optionValue;
                }
                loadedMapOptions.Sanitize();
            }
        }

        public override void Initialize() {
            Options.Clear();
        }

        public LoadedMapOptions GetCurrentMapOptions() {
            var stageKey = Utility.GetCurrentStage().ToString();
            var mapOptions = MapOptions.Instance;
            if (mapOptions != null)
                stageKey = Utility.SceneNameToStage(mapOptions.gameObject.scene.name).ToString();
            if (Options.TryGetValue(stageKey, out var loadedMapOptions))
                return loadedMapOptions;
            var mapOpts = new LoadedMapOptions();
            mapOpts.MakeDefault();
            Options[stageKey] = mapOpts;
            return mapOpts;
        }
    }
}
