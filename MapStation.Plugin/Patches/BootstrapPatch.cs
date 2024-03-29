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
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Bootstrap.LaunchGame))]
    private static bool LaunchGame_Prefix(Bootstrap __instance) {
        if (MapStationConfig.Instance.QuickLaunchValue != "") {
            // This assumes that AssetPatch already added our custom map to StageEnum, will error if it's typo'd
            __instance.StartCoroutine(__instance.SetupGameToStage(StageEnum.GetMapId(MapStationConfig.Instance.QuickLaunchValue)));
            return false;
        }
        return true;
    }

}
