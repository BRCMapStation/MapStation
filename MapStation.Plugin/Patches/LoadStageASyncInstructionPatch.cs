using System;
using HarmonyLib;
using Reptile;
using UnityEngine;

namespace MapStation.Plugin.Patches;

[HarmonyPatch(typeof(LoadStageASyncInstruction))]
class LoadStageASyncInstructionPatch {
    [HarmonyPrefix]
    [HarmonyPatch(MethodType.Constructor)]
    [HarmonyPatch(new Type[] { typeof(Stage), typeof(bool), typeof(float) })]
    private static void LoadStageASyncInstruction_ctor_Prefix(ref Stage stage) {
        // If the game tries to load a custom map that isn't in our database,
        // send them to hideout instead of soft-locking
        if((StageEnum.IsValidMapId(stage) && !StageEnum.IsKnownMapId(stage)) || stage == Stage.NONE) {
            Debug.Log($"Invariant violated! Attempting to load invalid/unrecognized stage int={stage} ToString()=\"{stage.ToString()}\"  This should never happen, because other patches should redirect to hideout before we get here.");
            stage = Stage.hideout;
        }
    }
}