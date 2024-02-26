using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Reptile;

namespace MapStation.Plugin.Patches {
    [HarmonyPatch(typeof(VideoSettingsManager))]
    internal static class VideoSettingsManagerPatch {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(VideoSettingsManager.ApplyQualitySettings))]
        private static void ApplyQualitySettings_Postfix(VideoSettings videoSettings) {
            MapOverrides.OverrideQualitySettings(Core.Instance.BaseModule.CurrentStage, videoSettings);
        }
    }
}
