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
            if (!__instance.isAI) {
                winterPlayer.ToyMachineAbility = new ToyMachineAbility(__instance);
            }
#if WINTER_DEBUG
            if(WinterConfig.Instance.DisableKBMInputValue) {
                KBMInputDisabler.Disable();
            }
#endif
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Player.EndGraffitiMode))]
        private static void EndGraffitiMode_Postfix(Player __instance, GraffitiSpot graffitiSpot) {
            if (graffitiSpot.state != GraffitiState.FINISHED)
                return;
            if (__instance.isAI)
                return;
            if (graffitiSpot.ClaimedByPlayableCrew()) {
                var toyGraff = ToyGraffitiSpot.Get(graffitiSpot);
                if (toyGraff == null)
                    return;
                toyGraff.ToyMachine.FinishToyLine();
                toyGraff.ToyMachine.TeleportPlayerToExit(true);
                graffitiSpot.allowRedo = true;
            }
        }
    }
}
