using HarmonyLib;
using Reptile;
using Rewired;
using UnityEngine;

namespace MapStation.Plugin.Patches {
    [HarmonyPatch(typeof(SkateboardScrewPole))]
    public class SkateboardScrewPolePatch {

        // Goals:
        // Use local position and rotation, not global,
        // So that parent GameObject can be animated to move & rotate.
        
        [HarmonyPostfix]
        [HarmonyPatch(nameof(SkateboardScrewPole.Awake))]
        public static void Awake_Postfix(SkateboardScrewPole __instance) {
            __instance.poleStartPos = __instance.pole.transform.localPosition;
            __instance.startRot = __instance.pole.transform.localRotation;
            Debug.Log("Awake_Postfix ran");
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(nameof(SkateboardScrewPole.Move))]
        public static bool Move_Prefix(SkateboardScrewPole __instance, float distance)
        {
            // Replaces vanilla implementation entirely; return false from *all* code paths!
            if (distance == 0f)
            {
                __instance.audioManager.StopLoopingSfx(__instance.audioSource);
            }
            else if (distance < 0f)
            {
                if (__instance.pole.localPosition.y <= __instance.poleStartPos.y)
                {
                    __instance.pole.localPosition = __instance.poleStartPos;
                    __instance.pole.localRotation = __instance.startRot;
                    __instance.reset = false;
                    __instance.audioManager.StopLoopingSfx(__instance.audioSource);
                    return false;
                }
                __instance.pole.Rotate(new Vector3(0f, distance * -800f * Core.dt, 0f));
                __instance.pole.localPosition += Vector3.up * distance * Core.dt;
                if (!__instance.audioSource.isPlaying)
                {
                    __instance.audioManager.PlaySfxUILooping(SfxCollectionID.EnvironmentSfx, AudioClipID.ScrewPoleMoving, __instance.audioSource);
                }
            }
            else if (__instance.pole.localPosition.y < __instance.maxPoint.localPosition.y)
            {
                __instance.pole.Rotate(new Vector3(0f, distance * -800f * Core.dt, 0f));
                __instance.pole.localPosition += Vector3.up * distance * Core.dt;
                if (!__instance.audioSource.isPlaying)
                {
                    __instance.audioManager.PlaySfxUILooping(SfxCollectionID.EnvironmentSfx, AudioClipID.ScrewPoleMoving, __instance.audioSource);
                }
            }
            return false;
        }
    }
}
