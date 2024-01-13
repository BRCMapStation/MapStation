using MapStation.API;
using MapStation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapStation.Plugin.API {
    public class CustomStage : ICustomStage {
        public string DisplayName => MapEntry.Properties.displayName;
        public string InternalName => MapEntry.internalName;
        public string AuthorName => MapEntry.Properties.authorName;
        public int StageID => (int)MapEntry.stageId;

        public PluginMapDatabaseEntry MapEntry = null;

        public CustomStage(PluginMapDatabaseEntry mapEntry) {
            MapEntry = mapEntry;
        }
    }
}
