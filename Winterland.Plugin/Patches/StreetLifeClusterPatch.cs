using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using Winterland.MapStation.Common.VanillaAssets;
using HarmonyLib;

namespace Winterland.Plugin.Patches {
    [HarmonyPatch(typeof(StreetLifeCluster))]
    internal class StreetLifeClusterPatch {

        // Always keep streetlife inactive if we deleted it with MapStation.
        [HarmonyPrefix]
        [HarmonyPatch(nameof(StreetLifeCluster.UpdateStreetLifeCluster))]
        private static bool UpdateStreetLifeCluster_Prefix(StreetLifeCluster __instance) {
            var goDeleter = DeleteVanillaGameObjectsV1.Instance;
            if (goDeleter == null)
                return true;
            if (goDeleter.IsDisabled(__instance.gameObject)) {
                __instance.SetClusterActive(false);
                return false;
            }
            return true;
        }
    }
}
