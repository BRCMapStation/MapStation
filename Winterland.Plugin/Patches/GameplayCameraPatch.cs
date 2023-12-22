using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using Winterland.Common;
using UnityEngine;
using HarmonyLib;

namespace Winterland.Plugin.Patches {
    [HarmonyPatch(typeof(GameplayCamera))]
    internal class GameplayCameraPatch {
        private static float OriginalDragDistanceDefault;
        private static float OriginalDragDistanceMax;
        private static float OriginalCameraHeight;
        private static float OriginalCameraFOV;

        private const float CameraTransitionSpeed = 5f;

        [HarmonyPostfix]
        [HarmonyPatch(nameof(GameplayCamera.Awake))]
        private static void Awake_Postfix(GameplayCamera __instance) {
            OriginalDragDistanceDefault = __instance.dragDistanceDefault;
            OriginalDragDistanceMax = __instance.dragDistanceMax;
            OriginalCameraHeight = __instance.defaultCamHeight;
            OriginalCameraFOV = __instance.cam.fieldOfView;
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(GameplayCamera.UpdateCamera))]
        private static void UpdateCamera_Postfix(GameplayCamera __instance) {
            var targetDragDistanceDefault = OriginalDragDistanceDefault;
            var targetDragDistanceMax = OriginalDragDistanceMax;
            var targetCameraHeight = OriginalCameraHeight;
            var targetCameraFOV = OriginalCameraFOV;

            var player = WinterPlayer.GetLocal();

            if (player == null)
                return;

            if (player.CurrentCameraZoomZone != null) {
                var zoomZone = player.CurrentCameraZoomZone;
                if (zoomZone.ChangeCameraPosition) {
                    targetDragDistanceDefault = zoomZone.CameraDragDistanceDefault;
                    targetDragDistanceMax = zoomZone.CameraDragDistanceMax;
                    targetCameraHeight = zoomZone.CameraHeight;
                }
                if (zoomZone.ChangeCameraFOV && !Plugin.DynamicCameraInstalled)
                    targetCameraFOV = zoomZone.CameraFOV;
            }

            var delta = Core.dt * CameraTransitionSpeed;
            __instance.dragDistanceDefault = Mathf.Lerp(__instance.dragDistanceDefault, targetDragDistanceDefault, delta);
            __instance.dragDistanceMax = Mathf.Lerp(__instance.dragDistanceMax, targetDragDistanceMax, delta);
            __instance.defaultCamHeight = Mathf.Lerp(__instance.defaultCamHeight, targetCameraHeight, delta);
            var fov = __instance.cam.fieldOfView;
            fov = Mathf.Lerp(fov, targetCameraFOV, delta);
            __instance.cam.fieldOfView = fov;
        }
    }
}
