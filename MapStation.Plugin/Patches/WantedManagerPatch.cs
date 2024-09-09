using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Reptile;

namespace MapStation.Plugin.Patches {
    [HarmonyPatch(typeof(WantedManager))]
    internal static class WantedManagerPatch {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(WantedManager.UpdateStageProgress))]
        private static void UpdateStageProgress_Postfix(WantedManager __instance) {
            if (MapDatabase.Instance.maps.TryGetValue(Core.Instance.BaseModule.CurrentStage, out var pluginMapEntry)) {
                var stageProgress = __instance.saveSlotData.GetCurrentStageProgress();
                stageProgress.policeAllowed = !pluginMapEntry.Properties.disableCops;
                if (pluginMapEntry.Properties.disableCops) {
                    __instance.BlockWantedStatus(true);
                } else {
                    __instance.BlockWantedStatus(false);
                }
            }
        }
    }
}
