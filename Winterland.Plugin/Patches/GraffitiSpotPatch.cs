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
            if (REPLessGraffiti.IsREPLess(__instance))
                return false;
            var toyGraff = ToyGraffitiSpot.Get(__instance);
            if (toyGraff != null)
                return false;
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GraffitiSpot.CanDoGraffiti))]
        private static bool CanDoGraffiti_Prefix(ref bool __result, Player byPlayer, GraffitiSpot __instance) {
            var toyGraff = ToyGraffitiSpot.Get(__instance);
            if (toyGraff != null && byPlayer.ability is not ToyMachineAbility) {
                __result = false;
                return false;
            }
            if (byPlayer.ability is ToyMachineAbility) {
                var winterPlayer = WinterPlayer.Get(byPlayer);
                if (winterPlayer != null) {
                    if (winterPlayer.CurrentToyLine == null) {
                        __result = false;
                        return false;
                    }
                    if (winterPlayer.CollectedToyParts != winterPlayer.CurrentToyLine.ToyParts.Length) {
                        __result = false;
                        return false;
                    }
                }
                var toyMachineAbility = byPlayer.ability as ToyMachineAbility;
                if (!toyMachineAbility.CanTag) {
                    __result = false;
                    return false;
                }
            }
            return true;
        }
    }
}
