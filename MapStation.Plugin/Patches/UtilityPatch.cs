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

            if(sceneName.StartsWith(StageEnum.MapNamePrefix)) {
                var mapName = sceneName.Substring(StageEnum.MapNamePrefixLength);
                if(StageEnum.MapIds.TryGetValue(mapName, out var mapId)) {
                    __result = mapId;
                    return;
                }
            } else if(sceneName.StartsWith(AssetNames.SceneBasenamePrefix)) {
                var mapName = sceneName.Substring(AssetNames.SceneBasenamePrefix.Length);
                if(StageEnum.MapIds.TryGetValue(mapName, out var mapId)) {
                    __result = mapId;
                    return;
                }
            }
            // TODO BundledMaps
            // TODO reserve an id for when parsing fails?
        }
    }
}
