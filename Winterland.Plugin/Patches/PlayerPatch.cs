using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using Winterland.Common;
using HarmonyLib;

namespace Winterland.Plugin.Patches {
    [HarmonyPatch(typeof(Player))]
    internal static class PlayerPatch {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(Player.Init))]
        private static void Init_Postfix(Player __instance) {
            var winterPlayer = __instance.gameObject.AddComponent<WinterPlayer>();
            winterPlayer.player = __instance;
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Player.FixedUpdatePlayer))]
        private static void FixedUpdatePlayer_Postfix(Player __instance) {
            var winterPlayer = WinterPlayer.Get(__instance);
            if (winterPlayer == null)
                return;
            if (!__instance.IsComboing()) {
                if (winterPlayer.CurrentToyLine != null)
                    winterPlayer.CurrentToyLine.Respawn();
                winterPlayer.CurrentToyLine = null;
                if (WinterUI.Instance != null && winterPlayer.Local) {
                    var toyLineUI = WinterUI.Instance.ToyLineUI;
                    toyLineUI.Visible = false;
                }
            }
        }
    }
}
