using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using HarmonyLib;
using MapStation.Plugin.Gameplay;

namespace MapStation.Plugin.Patches {
    [HarmonyPatch(typeof(LedgeClimbAbility))]
    internal static class LedgeClimbAbilityPatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(LedgeClimbAbility.CheckActivation))]
        private static bool CheckActivation_Prefix(LedgeClimbAbility __instance, ref bool __result) {
            var mpPlayer = MapStationPlayer.Get(__instance.p);
            if (mpPlayer.OnVertAir) {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
