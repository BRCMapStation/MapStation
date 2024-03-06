using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using HarmonyLib;
using UnityEngine;

namespace MapStation.Plugin.Patches {
    [HarmonyPatch(typeof(Mapcontroller))]
    internal static class MapcontrollerPatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(Mapcontroller.GetMap))]
        private static bool GetMap_Prefix(Stage stage, Map __result) {
            if (MapDatabase.Instance.maps.TryGetValue(stage, out var customStage)) {
                return false;
            }
            return true;
        }
    }
}
