using Reptile;
using HarmonyLib;
using Winterland.Plugin;
using MapStation.Common.Gameplay;
using UnityEngine.UIElements;

namespace MapStation.Plugin.Patches;

[HarmonyPatch(typeof(Player))]
internal static class PlayerPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Player.Init))]
    private static void Init_Postfix(Player __instance) {
        if(MapStationConfig.Instance.DisableKBMInputValue) {
            KBMInputDisabler.Disable();
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Player.CheckVert))]
    private static bool CheckVert(Player __instance, ref bool __result) {
        if (__instance.OnAnyGround() && __instance.motor.groundCollider.GetComponent<MapStationVert>() != null) {
            __instance.motor.groundDetection.groundLimit = 90f;
            __instance.OrientVisualInstant();
            UnityEngine.Debug.Log(__instance.motor.groundNormalVisual.ToString());
            //__result = true;
            return true;
        }
        __instance.motor.groundDetection.groundLimit = 60f;
        return true;
    }
}
