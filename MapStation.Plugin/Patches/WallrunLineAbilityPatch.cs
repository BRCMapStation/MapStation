using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using HarmonyLib;
using MapStation.Plugin.Gameplay;

namespace MapStation.Plugin.Patches {
    [HarmonyPatch(typeof(WallrunLineAbility))]
    internal static class WallrunLineAbilityPatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(WallrunLineAbility.OnStartAbility))]
        private static void OnStartAbility_Prefix(WallrunLineAbility __instance) {
            var player = __instance.p;
            var mpPlayer = MapStationPlayer.Get(player);
            if (mpPlayer.OnVertAir)
                mpPlayer.AirVertEnd();
        }
    }
}
