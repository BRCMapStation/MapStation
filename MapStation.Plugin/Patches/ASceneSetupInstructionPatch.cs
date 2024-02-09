using HarmonyLib;
using MapStation.Common;
using Reptile;
using UnityEngine;

namespace MapStation.Plugin.Patches;

[HarmonyPatch(typeof(ASceneSetupInstruction))]
public class ASceneSetupInstructionPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(ASceneSetupInstruction.SetSceneActive))]
    protected static void SetSceneActive_Prefix(ref string sceneToSetActive) {
        if (SceneNameMapper.Instance.Names.TryGetValue(sceneToSetActive, out var replacement)) {
            Log.Info($"{nameof(ASceneSetupInstruction)}.{nameof(ASceneSetupInstruction.SetSceneActive)} redirected from {sceneToSetActive} to {replacement}");
            sceneToSetActive = replacement;
        }
    }
}
