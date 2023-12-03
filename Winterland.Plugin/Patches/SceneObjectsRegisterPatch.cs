using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using HarmonyLib;
using Winterland.Common;

namespace Winterland.Plugin.Patches {
    [HarmonyPatch(typeof(SceneObjectsRegister))]
    internal class SceneObjectsRegisterPatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(SceneObjectsRegister.GetTotalGraffitiREP))]
        private static bool GetTotalGraffitiREP_Prefix(ref int __result, SceneObjectsRegister __instance) {
            var rep = 0;
            for (var i = 0; i < __instance.grafSpots.Count; i++) {
                if (!(__instance.grafSpots[i] is GraffitiSpotFinisher)) {
                    if (ToyGraffitiSpot.Get(__instance.grafSpots[i]) != null)
                        continue;
                    rep += GraffitiSpot.GetREP(__instance.grafSpots[i].size);
                }
            }
            __result = rep;
            return false;
        }
    }
}
