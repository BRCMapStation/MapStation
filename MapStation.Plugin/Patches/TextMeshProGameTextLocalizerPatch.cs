using HarmonyLib;
using Reptile;

namespace MapStation.Plugin.Patches;

[HarmonyPatch(typeof(TextMeshProGameTextLocalizer))]
class TextMeshProGameTextLocalizerPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(TextMeshProGameTextLocalizer.GetStageNameKey))]
    private static bool GetStageNameKey_Prefix(TextMeshProGameTextLocalizer __instance, Stage stageID, ref string __result) {
        if(StageEnum.IsValidMapId(stageID) || StageEnum.IsValidBundledMapId(stageID)) {
            __result = __instance.GetStageNameKey(Stage.hideout);
            return false;
        }
        return true;
    }
}