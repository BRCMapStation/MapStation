using HarmonyLib;
using MapStation.Common;
using Reptile;
using UnityEngine;

namespace MapStation.Plugin.Patches;

[HarmonyPatch(typeof(Utility))]
internal static class UtilityPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Utility.SceneNameToStage))]
    private static void SceneNameToStage_Postfix(ref Stage __result, string sceneName) {
        if(__result == Stage.NONE) {
            // HACK:
            // We are usually passed a Stage.ToString()-style value,
            // but occasionally with SceneManager.GetActiveScene().name.
            // We must handle both

            bool recognized = false;
            if(sceneName.StartsWith(StageEnum.MapNamePrefix)) {
                var mapName = sceneName.Substring(StageEnum.MapNamePrefixLength);
                if(StageEnum.MapIds.TryGetValue(mapName, out var mapId)) {
                    __result = mapId;
                    recognized = true;
                }
            } else if(sceneName.StartsWith(AssetNames.SceneBasenamePrefix)) {
                var mapName = sceneName.Substring(AssetNames.SceneBasenamePrefix.Length);
                if(StageEnum.MapIds.TryGetValue(mapName, out var mapId)) {
                    __result = mapId;
                    recognized = true;
                }
            }

            if(recognized) {
                Debug.Log($"Utility.SceneNameToStage: non-vanilla sceneName=\"{sceneName}\" parsed as int={__result}, Stage.ToString()=\"{__result.ToString()}\"");
                return;
            }
            
            // If we get here, it may be a custom map that we don't recognize because it's been uninstalled.
            // Send the player to hideout instead.
            if(sceneName != "NONE") {
                __result = Stage.hideout;
                Debug.Log($"Utility.SceneNameToStage: non-vanilla sceneName=\"{sceneName}\" unrecognized, redirecting to hideout.");
            }
        }
    }
}
