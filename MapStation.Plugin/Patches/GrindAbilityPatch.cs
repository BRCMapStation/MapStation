using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using HarmonyLib;
using MapStation.Plugin.Gameplay;

namespace MapStation.Plugin.Patches {
    [HarmonyPatch(typeof(GrindAbility))]
    internal static class GrindAbilityPatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(GrindAbility.SetToLine))]
        private static void SetToLine_Prefix(GrindAbility __instance) {
            var player = __instance.p;
            var mpPlayer = MapStationPlayer.Get(player);
            if (mpPlayer.OnVertGround || mpPlayer.OnVertAir || mpPlayer.HasVertBelow) {
                player.OrientVisualInstantReset();
            }
        }
    }
}
