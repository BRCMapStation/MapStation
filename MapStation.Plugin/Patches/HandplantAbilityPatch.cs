
using System;
using Reptile;
using HarmonyLib;
using MapStation.Plugin.Gameplay;
using UnityEngine;

namespace MapStation.Plugin.Patches {
    [HarmonyPatch(typeof(HandplantAbility))]
    internal static class HandplantAbilityPatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(HandplantAbility.FixedUpdateAbility))]
        private static void FixedUpdateAbility_Prefix(HandplantAbility __instance) {
            var mpAbility = MapStationPlayer.Get(__instance.p).MapStationHandplantAbility;
            // Is a prefix, so we can assume it's still the active ability
            // If changed to postfix, add logic to check if still active
            mpAbility.FixedUpdateAbility();
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(HandplantAbility.SetToPole), new Type[] {typeof(Vector3), typeof(SkateboardScrewPole)})]
        private static void SetToPole_Prefix(HandplantAbility __instance, Vector3 polePoint, SkateboardScrewPole setScrewPole) {
            var player = __instance.p;
            var mpPlayer = MapStationPlayer.Get(player);
            var mpAbility = mpPlayer.MapStationHandplantAbility;
            mpAbility.SetOnScrewpole(setScrewPole);
        }
    }
}
