using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Reptile;
using UnityEngine;
using Winterland.Common;

namespace Winterland.Plugin.Patches {
    [HarmonyPatch(typeof(Bootstrap))]
    internal static class BootstrapPatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(Bootstrap.LaunchGame))]
        private static bool LaunchGame_Prefix(Bootstrap __instance) {
#if WINTER_DEBUG
            if (!WinterConfig.Instance.QuickLaunch.Value)
                return true;
            Plugin.UpdateEvent += QuickLaunchUpdate;
            __instance.StartCoroutine(__instance.SetupGameToStage(Stage.square));
#endif
            return false;
        }

        private static void QuickLaunchUpdate() {
            if(Input.GetKeyDown(KeyCode.F5)) {
                GameObject.FindFirstObjectByType<Bootstrap>().StartCoroutine(QuickLaunchReloadStage());
            }
        }
        private static IEnumerator QuickLaunchReloadStage() {
            yield return null;
            WinterAssets.Instance.ReloadBundles();
            Core.Instance.BaseModule.SwitchStage(Utility.GetCurrentStage());
        }
    }
}
