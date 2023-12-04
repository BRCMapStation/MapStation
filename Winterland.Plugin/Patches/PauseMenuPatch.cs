using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using HarmonyLib;
using Winterland.Common;

namespace Winterland.Plugin.Patches {
    [HarmonyPatch(typeof(PauseMenu))]
    internal class PauseMenuPatch {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(PauseMenu.RefreshLabels))]
        private static void RefreshLabels_Postfix(PauseMenu __instance) {
            var currentObjective = WinterProgress.Instance?.LocalProgress?.Objective;
            if (currentObjective == null)
                return;
            if (!currentObjective.Test())
                return;
            if (string.IsNullOrEmpty(currentObjective.Description))
                return;
            __instance.currentObjectiveText.text = currentObjective.Description;
        }
    }
}
