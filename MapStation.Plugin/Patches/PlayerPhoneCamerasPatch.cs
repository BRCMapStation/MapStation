using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MapStation.Common.Runtime;
using Reptile;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace MapStation.Plugin.Patches {
    [HarmonyPatch(typeof(PlayerPhoneCameras))]
    internal class PlayerPhoneCamerasPatch {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerPhoneCameras.Awake))]
        private static void Awake_Postfix(PlayerPhoneCameras __instance) {
            var cameraOverride = MapStationCameraOverride.Instance;
            if (cameraOverride == null) return;
            if (!cameraOverride.AlsoAffectPhoneCamera) return;
            var cams = __instance.GetComponentsInChildren<Camera>();
            foreach(var cam in cams) {
                cameraOverride.ApplyToCamera(cam);
                var postFXLayer = cam.GetComponent<PostProcessLayer>();
                if (postFXLayer != null)
                    cam.GetComponent<PostProcessLayer>().volumeLayer |= 1 << Layers.Phone;
            }
        }
    }
}
