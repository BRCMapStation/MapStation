using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Reptile;

namespace Winterland.Plugin.Patches {
    [HarmonyPatch(typeof(Bootstrap))]
    internal static class BootstrapPatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(Bootstrap.LaunchGame))]
        private static bool LaunchGame_Prefix(Bootstrap __instance) {
            if (!Plugin.WinterConfig.QuickLaunch.Value)
                return true;
            __instance.StartCoroutine(__instance.SetupGameToStage(Stage.square));
            return false;
        }
    }
}
