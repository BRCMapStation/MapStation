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
            var beatTheGame = __instance.saveManager.CurrentSaveSlot.CurrentStoryObjective == Story.ObjectiveID.HangOut;
            if (!beatTheGame && currentObjective.OnlyInPostGame)
                return;
            var currentStage = Core.Instance.BaseModule.CurrentStage.ToString();
            if (currentObjective.HasSpecificStage && currentObjective.Stage != currentStage)
                return;
            if (string.IsNullOrEmpty(currentObjective.Description))
                return;
            __instance.currentObjectiveText.text = currentObjective.Description;
        }
    }
}
