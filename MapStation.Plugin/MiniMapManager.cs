using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapStation.Common;
using Reptile;
using UnityEngine;

namespace MapStation.Plugin {
    public static class MiniMapManager {
        public static Map CreateMapForCustomStage(BaseMapDatabaseEntry mapEntry) {
            var map = ScriptableObject.CreateInstance<Map>();
            return map;
        }
    }
}
