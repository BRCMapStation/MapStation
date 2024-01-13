using MapStation.API;
using Reptile;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MapStation.Plugin.API {
    public class MapStationAPI : IMapStationAPI {
        // Could maybe optimize this with a custom enumerable class or something that doesn't copy the whole thing everytime it's queried.
        public ReadOnlyCollection<ICustomStage> CustomStages {
            get {
                var customStageList = new List<ICustomStage>();
                var stages = MapDatabase.maps.Values;
                foreach(var stage in stages) {
                    var customStage = new CustomStage(stage);
                    customStageList.Add(customStage);
                }
                return customStageList.AsReadOnly();
            }
        }
        public MapDatabase MapDatabase = null;
        public MapStationAPI(MapDatabase mapDatabase) {
            MapDatabase = mapDatabase;
        }

        public ICustomStage GetCustomStageByID(int stageID) {
            if (MapDatabase.maps.TryGetValue((Stage) stageID, out var result))
                return new CustomStage(result);
            return null;
        }

        public int GetStageID(string stageInternalName) {
            return (int)StageEnum.HashMapName(stageInternalName);
        }
    }
}
