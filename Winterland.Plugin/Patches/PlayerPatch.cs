using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using Winterland.Mono;
using HarmonyLib;

namespace Winterland.Plugin.Patches {
    [HarmonyPatch(typeof(Player))]
    internal static class PlayerPatch {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(Player.Init))]
        private static void Init_Postfix(Player __instance) {
            __instance.gameObject.AddComponent<WinterPlayer>();
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
            }
        }
    }
}
