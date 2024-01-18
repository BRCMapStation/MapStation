using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using HarmonyLib;
using MapStation.Plugin.Gameplay;
using Rewired;

namespace MapStation.Plugin.Patches {
    [HarmonyPatch(typeof(AirTrickAbility))]
    internal static class AirTrickAbilityPatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(AirTrickAbility.SetupBoostTrick))]
        private static bool SetupBoostTrick_Prefix(AirTrickAbility __instance) {
            var player = __instance.p;
            var mpPlayer = MapStationPlayer.Get(player);
            if (mpPlayer.OnVertAir) {
                player.PlayAnim(__instance.airBoostTrickHashes[__instance.curTrick], true, false, 0f);
                player.PlayVoice(AudioClipID.VoiceBoostTrick, VoicePriority.BOOST_TRICK, true);
                player.ringParticles.Emit(1);
                __instance.trickType = AirTrickAbility.TrickType.BOOST_TRICK;
                __instance.duration *= 1.5f;
                player.AddBoostCharge(-player.boostTrickCost);
                return false;
            }
            return true;
        }
    }
}
