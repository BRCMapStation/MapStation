using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MapStation.Plugin.Gameplay;
using Reptile;

namespace MapStation.Plugin.Patches {
    [HarmonyPatch(typeof(BoostAbility))]
    internal static class BoostAbilityPatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(BoostAbility.SetState))]
        private static void SetState_Prefix(BoostAbility __instance, BoostAbility.State setState) {
            var mpPlayer = MapStationPlayer.Get(__instance.p);
            if (mpPlayer.OnVertAir && setState == BoostAbility.State.BOOST)
                mpPlayer.AirVertEnd();
        }
    }
}
