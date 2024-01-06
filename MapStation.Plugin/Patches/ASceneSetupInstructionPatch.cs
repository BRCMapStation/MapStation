using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Reptile;
using UnityEngine;

namespace MapStation.Plugin.Patches;

[HarmonyPatch(typeof(ASceneSetupInstruction))]
internal static class ASceneSetupInstructionPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(ASceneSetupInstruction.SetSceneActive))]
    private static void SetSceneActive_Prefix(ref string sceneToSetActive) {
        if (sceneToSetActive == "mapstation/cspotcode.deatheggzone")
            sceneToSetActive = "Scene";
    }
}
