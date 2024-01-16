using Reptile;
using HarmonyLib;
using Winterland.Plugin;
using MapStation.Common.Gameplay;
using UnityEngine.UIElements;
using MapStation.Plugin.Gameplay;
using UnityEngine;

namespace MapStation.Plugin.Patches;

[HarmonyPatch(typeof(Player))]
internal static class PlayerPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Player.Init))]
    private static void Init_Postfix(Player __instance) {
        if(MapStationConfig.Instance.DisableKBMInputValue) {
            KBMInputDisabler.Disable();
        }
        __instance.gameObject.AddComponent<MapStationPlayer>();
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Player.OnLanded))]
    private static bool OnLanded_Prefix(Player __instance) {
        var mpPlayer = MapStationPlayer.Get(__instance);
        if (mpPlayer.OnVertGround && Vector3.Angle(__instance.motor.groundNormal, Vector3.up) >= MapStationPlayer.MinimumGroundVertAngle)
            return false;
        return true;
    }

    /*
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
    }*/
}
