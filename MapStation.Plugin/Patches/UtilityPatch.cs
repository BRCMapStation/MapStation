using HarmonyLib;
using Reptile;

namespace MapStation.Plugin.Patches;

[HarmonyPatch(typeof(Utility))]
internal static class UtilityPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Utility.SceneNameToStage))]
    private static void SceneNameToStage_Postfix(ref Stage __result, string sceneName) {
        if(__result == Stage.NONE) {
            if(sceneName.StartsWith(StageEnum.MapNamePrefix)) {
                var mapName = sceneName.Substring(StageEnum.MapNamePrefixLength);
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
