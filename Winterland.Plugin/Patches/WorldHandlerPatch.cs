using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using Winterland.Common;
using HarmonyLib;

namespace Winterland.Plugin.Patches {
    [HarmonyPatch(typeof(WorldHandler))]
    internal class WorldHandlerPatch {

        [HarmonyPrefix]
        [HarmonyPatch(nameof(WorldHandler.SetNPCAvailabilityBasedOnPlayer))]
        private static bool SetNPCAvailabilityBasedOnPlayer_Prefix(WorldHandler __instance) {
            var baseCharacter = WinterCharacters.GetBaseCharacter(__instance.currentPlayer.character);
            if (baseCharacter == Characters.NONE)
                return true;
            foreach(var npc in __instance.sceneObjectsRegister.NPCs) {
                npc.SetAvailable(npc.character != baseCharacter);
            }
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(WorldHandler.CharacterIsAlreadyNPCInScene))]
        private static bool CharacterIsAlreadyNPCInScene_Prefix(ref Characters c, WorldHandler __instance, ref bool __result) {
            var baseCharacter = WinterCharacters.GetBaseCharacter(c);
            if (baseCharacter != Characters.NONE)
                c = baseCharacter;

            var playerBaseCharacter = WinterCharacters.GetBaseCharacter(__instance.currentPlayer.character);
            if (playerBaseCharacter == Characters.NONE)
                playerBaseCharacter = __instance.currentPlayer.character;

            if (playerBaseCharacter == c) {
                __result = true;
                return false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(WorldHandler.SetNPCAvailabilityBasedOnCharacter))]
        private static void SetNPCAvailabilityBasedOnCharacter_Prefix(ref Characters oldChar, ref Characters newChar) {
            var oldCharBase = WinterCharacters.GetBaseCharacter(oldChar);
            if (oldCharBase != Characters.NONE)
                oldChar = oldCharBase;

            var newCharBase = WinterCharacters.GetBaseCharacter(newChar);
            if (newCharBase != Characters.NONE)
                newChar = newCharBase;
        }
    }
}
