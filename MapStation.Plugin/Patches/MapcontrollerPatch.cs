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
        // MapMaterial could be null in a custom stage.
        [HarmonyPrefix]
        [HarmonyPatch(nameof(Mapcontroller.SetMapGradient))]
        private static bool SetMapGradient_Prefix(Mapcontroller __instance) {
            if (__instance.mapMaterial == null)
                return false;
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(Mapcontroller.GetMap))]
        private static bool GetMap_Prefix(Stage stage, ref Map __result) {
            if (MapDatabase.Instance.maps.TryGetValue(stage, out var customStage)) {
                if (!MiniMapManager.TryCreateMapForCustomStage(customStage, out var map))
                    return true;
                __result = map;
                return false;
            }
            return true;
        }
    }
}
