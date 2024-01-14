using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;
using HarmonyLib;

namespace MapStation.Plugin.Patches;

[HarmonyPatch(typeof(GameplayCamera))]
internal static class GameplayCameraPatch {

    private static readonly int[] DebugLayers = [
        Layers.CameraIgnore,
        Layers.TriggerDetectPlayer
    ];

    [HarmonyPostfix]
    [HarmonyPatch(nameof(GameplayCamera.Awake))]
    private static void Awake_Postfix(GameplayCamera __instance) {
        if (MapStationConfig.Instance.ShowDebugShapesValue) {
            foreach (var layer in DebugLayers) {
                __instance.cam.cullingMask |= (1 << layer);
            }
        }
    }
}
