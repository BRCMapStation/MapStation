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
        [HarmonyPostfix]
        [HarmonyPatch(nameof(GraffitiSpot.SetState))]
        private static void SetState_Postfix(GraffitiState setState, GraffitiSpot __instance) {
            if (setState == GraffitiState.FINISHED && __instance.ClaimedByPlayableCrew()) {
                var toyGraff = ToyGraffitiSpot.Get(__instance);
                if (toyGraff == null)
                    return;
                toyGraff.ToyMachine.FinishToyLine();
            }
        }

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
