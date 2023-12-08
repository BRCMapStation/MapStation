using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using Winterland.Common;
using HarmonyLib;

namespace Winterland.Plugin.Patches {
    [HarmonyPatch(typeof(GraffitiSpot))]
    internal class GraffitiSpotPatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(GraffitiSpot.GiveRep))]
        private static bool GiveRep_Prefix(GraffitiSpot __instance) {
            var toyGraff = ToyGraffitiSpot.Get(__instance);
            if (toyGraff != null)
                return false;
            return true;
        }
    }
}
