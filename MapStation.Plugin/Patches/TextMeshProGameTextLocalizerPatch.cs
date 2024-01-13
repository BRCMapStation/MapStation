using HarmonyLib;
using Reptile;

namespace MapStation.Plugin.Patches;

[HarmonyPatch(typeof(TextMeshProGameTextLocalizer))]
class TextMeshProGameTextLocalizerPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(TextMeshProGameTextLocalizer.GetStageNameKey))]
    private static bool GetStageNameKey_Prefix(TextMeshProGameTextLocalizer __instance, Stage stageID, ref string __result) {
        if (MapDatabase.Instance.maps.TryGetValue(stageID, out var map)) {
            __result = map.Properties.displayName;
            return false;
        }
        return true;
    }
}
