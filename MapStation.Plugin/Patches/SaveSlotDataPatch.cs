using HarmonyLib;
using Reptile;

namespace MapStation.Plugin.Patches;

[HarmonyPatch(typeof(SaveSlotData))]
internal static class SaveSlotDataPatch { 
    [HarmonyPostfix]
    [HarmonyPatch(nameof(SaveSlotData.GetStageProgress))]
    private static void GetStageProgress_Postfix(Stage stage, ref StageProgress __result) {
        if (__result == null && StageEnum.IsKnownMapId(stage)) {
            // This is a custom stage; progress is stored outside the save file
            __result = StageProgresses.Instance.GetOrCreateForStage(stage);
        }
    }
}
