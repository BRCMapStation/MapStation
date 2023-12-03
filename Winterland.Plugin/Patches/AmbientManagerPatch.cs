using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using Winterland.Common;
using HarmonyLib;

namespace Winterland.Plugin.Patches {
    [HarmonyPatch(typeof(AmbientManager))]
    internal class AmbientManagerPatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(AmbientManager.Update))]
        private static bool Update_Prefix(AmbientManager __instance) {
            if (AmbientOverride.Instance == null)
                return true;
            __instance.ApplyAmbientChange(AmbientOverride.Instance.LightColor, AmbientOverride.Instance.ShadowColor);
            return false;
        }
    }
}
