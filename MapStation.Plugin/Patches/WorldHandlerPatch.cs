using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MapStation.Common.Runtime;
using Reptile;
using UnityEngine;

namespace MapStation.Plugin.Patches {
    [HarmonyPatch(typeof(WorldHandler))]
    internal static class WorldHandlerPatch {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(WorldHandler.SetCurrentCamera))]
        private static void SetCurrentCamera_Postfix(Camera camera) {
            if (MapStationCameraOverride.Instance == null) return;
            MapStationCameraOverride.Instance.ApplyToCamera(camera);
        }
    }
}
