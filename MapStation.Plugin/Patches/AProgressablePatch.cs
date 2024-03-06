using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Reptile;
using UnityEngine;

namespace MapStation.Plugin.Patches {
    [HarmonyPatch(typeof(AProgressable))]
    internal static class AProgressablePatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(AProgressable.InitProgressable))]
        private static bool InitProgressable_Prefix(AProgressable __instance, StageProgress stageProgress) {
            __instance.hasSaveData = stageProgress.HasProgressableData(__instance.uid);
            if (__instance.hasSaveData) {
                __instance.progressableData = stageProgress.GetProgressableData(__instance.uid);
                try {
                    __instance.ReadFromData();
                }catch(InvalidCastException e) {
                    Debug.LogWarning($"Invalid data for progressable with UID {__instance.uid}, type {__instance.GetType()} in Stage {((Stage)stageProgress.stageID).ToString()}. Setting default data.");
                    __instance.SetDefaultData(stageProgress);
                    return false;
                }
                return false;
            }
            __instance.SetDefaultData(stageProgress);
            return false;
        }
    }
}
