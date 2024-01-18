using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using MapStation.Plugin.Gameplay;
using HarmonyLib;

namespace MapStation.Plugin.Patches {
    [HarmonyPatch(typeof(AirDashAbility))]
    internal static class AirDashAbilityPatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(AirDashAbility.OnStartAbility))]
        private static void OnStartAbility_Prefix(AirDashAbility __instance) {
            var mpPlayer = MapStationPlayer.Get(__instance.p);
            if (mpPlayer.OnVertAir)
                mpPlayer.AirVertEnd();
        }
    }
}
