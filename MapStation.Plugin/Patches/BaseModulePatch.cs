using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using HarmonyLib;

namespace MapStation.Plugin.Patches {
    [HarmonyPatch(typeof(BaseModule))]
    internal static class BaseModulePatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(BaseModule.SetupNewStage))]
        private static bool SetupNewStage_Prefix() {
            if (!StagePrefabHijacker.Loaded && StagePrefabHijacker.Active) {
                StagePrefabHijacker.Run();
                StagePrefabHijacker.Log("Hideout resources loaded - switching to intended stage now.");
                StagePrefabHijacker.Loaded = true;
                StagePrefabHijacker.Active = false;
                Core.Instance.BaseModule.SwitchStage(StagePrefabHijacker.ActualTargetStage);
                return false;
            }
            return true;
        }
    }
}
