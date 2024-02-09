using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;
using HarmonyLib;
using MapStation.Plugin.Gameplay;
using MapStation.Plugin.Tools;

namespace MapStation.Plugin.Patches;

[HarmonyPatch(typeof(GameplayCamera))]
internal static class GameplayCameraPatch {

    [HarmonyPostfix]
    [HarmonyPatch(nameof(GameplayCamera.Awake))]
    private static void Awake_Postfix(GameplayCamera __instance) {
        HiddenShapes.Camera = __instance;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(GameplayCamera.UpdateCamera))]
    private static void UpdateCamera_Postfix(GameplayCamera __instance) {
        if (!__instance.player || !__instance.isActiveAndEnabled) {
            return;
        }
        if (__instance.hitpause > 0f && __instance.player.GetVelocity() == Vector3.zero) {
            return;
        }
        var mpPlayer = MapStationPlayer.Get(__instance.player);
        if (__instance.cameraMode == __instance.cameraModeDrag && mpPlayer.OnVertAir)
            __instance.SetCameraMode(mpPlayer.VertCameraMode, false);
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(GameplayCamera.SetCameraMode))]
    [HarmonyPatch(new Type[] { typeof(CameraMode), typeof(bool) })]
    private static bool SetCameraMode_Prefix(GameplayCamera __instance, ref CameraMode mode) {
        var player = __instance.player;
        var mpPlayer = MapStationPlayer.Get(player);
        if (mode is not CameraModeDrag) return true;
        if (mpPlayer.OnVertAir) {
            mode = mpPlayer.VertCameraMode;
            if (__instance.cameraMode == mpPlayer.VertCameraMode) return false;
        }
        return true;
    }
}
