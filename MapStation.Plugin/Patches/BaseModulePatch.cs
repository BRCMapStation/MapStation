using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using HarmonyLib;
using MapStation.Common;

namespace MapStation.Plugin.Patches {
    [HarmonyPatch(typeof(BaseModule))]
    internal static class BaseModulePatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(BaseModule.SetupNewStage))]
        private static bool SetupNewStage_Prefix(Stage newStage) {
            if (!StagePrefabHijacker.Loaded && StagePrefabHijacker.Active) {
                StagePrefabHijacker.RunOnHijackStage();
                StagePrefabHijacker.Log("Hideout resources loaded - switching to intended stage now.");
                StagePrefabHijacker.Loaded = true;
                StagePrefabHijacker.Active = false;
                Core.Instance.BaseModule.SwitchStage(StagePrefabHijacker.ActualTargetStage);
                return false;
            }
            if (StagePrefabHijacker.Loaded && MapDatabase.Instance.maps.ContainsKey(newStage)) {
                StagePrefabHijacker.RunOnCustomStage();
            }
            StagePrefabHijacker.Loaded = false;
            return true;
        }
    }
}
