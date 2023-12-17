using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using HarmonyLib;
using Winterland.Common;
using System.Runtime.CompilerServices;

namespace Winterland.Plugin.Patches {
    [HarmonyPatch(typeof(GraffitiGame))]
    internal class GraffitiGamePatch {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(GraffitiGame.Init))]
        private static void Init_Postfix(GraffitiSpot g, GraffitiGame __instance) {
            var toyGraff = ToyGraffitiSpot.Get(g);
            if (toyGraff == null)
                return;
            __instance.distanceToGrafSpot = 1.75f;
            var interiorLighting = toyGraff.ToyMachine.InteriorLighting;
            var ambientOverride = AmbientOverride.Instance;
            if (interiorLighting != null && ambientOverride != null) {
                ambientOverride.TransitionAmbient(interiorLighting);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(GraffitiGame.SetState))]
        private static void SetState_Postfix(GraffitiGame.GraffitiGameState setState, GraffitiGame __instance) {
            var toyGraff = ToyGraffitiSpot.Get(__instance.gSpot);
            if (toyGraff == null)
                return;
            if (setState == GraffitiGame.GraffitiGameState.SHOW_PIECE) {
                __instance.showPieceCamSpeed = 5f;
                var player = WinterPlayer.GetLocal();
                if (player.CurrentToyLine != null) {
                    toyGraff.ToyMachine.ShowBuiltToy(player.CurrentToyLine.Toy, __instance.grafArt);
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(GraffitiGame.End))]
        private static void End_Postfix(GraffitiGame __instance) {
            var toyGraff = ToyGraffitiSpot.Get(__instance.gSpot);
            if (toyGraff == null)
                return;
            toyGraff.ToyMachine.HideBuiltToy();
            var interiorLighting = toyGraff.ToyMachine.InteriorLighting;
            var ambientOverride = AmbientOverride.Instance;
            if (interiorLighting != null && ambientOverride != null) {
                ambientOverride.StopAmbient(interiorLighting);
            }
        }
    }
}
