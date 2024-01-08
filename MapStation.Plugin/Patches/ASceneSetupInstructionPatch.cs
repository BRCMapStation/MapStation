using HarmonyLib;
using Reptile;
using UnityEngine;

namespace MapStation.Plugin.Patches;

[HarmonyPatch(typeof(ASceneSetupInstruction))]
public class ASceneSetupInstructionPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(ASceneSetupInstruction.SetSceneActive))]
    protected static void SetSceneActive_Prefix(ref string sceneToSetActive) {
        if (SceneNameMapper.Instance.Mappings.TryGetValue(sceneToSetActive, out var replacement)) {
            Debug.Log($"{nameof(ASceneSetupInstruction)}.{nameof(ASceneSetupInstruction.SetSceneActive)} redirected from {sceneToSetActive} to {replacement}");
            sceneToSetActive = replacement;
        }
    }
}
