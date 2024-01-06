using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Reptile;
using UnityEngine;
using MapStation.Common;
using System.Runtime.InteropServices;

namespace MapStation.Plugin.Patches;

[HarmonyPatch(typeof(Bootstrap))]
internal static class BootstrapPatch {
    // HACK hardcoded for testing, remove this
    public const string mapInternalName = "cspotcode.deatheggzone";

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Bootstrap.LaunchGame))]
    private static bool LaunchGame_Prefix(Bootstrap __instance) {
        if (!MapStationConfig.Instance.QuickLaunchValue)
            return true;
        Plugin.UpdateEvent += QuickLaunchUpdate;

        // HACK this assumes that AssetPatch already added our custom map to StageEnum
        __instance.StartCoroutine(__instance.SetupGameToStage(StageEnum.GetMapId(BootstrapPatch.mapInternalName)));
        return false;
    }

    private static void QuickLaunchUpdate() {
        if(Input.GetKeyDown(KeyCode.F5)) {
            GameObject.FindFirstObjectByType<Bootstrap>().StartCoroutine(QuickLaunchReloadStage());
        }
    }
    private static IEnumerator QuickLaunchReloadStage() {
        yield return null;
        Core.Instance.BaseModule.SwitchStage(StageEnum.FirstMapId);
    }
}
