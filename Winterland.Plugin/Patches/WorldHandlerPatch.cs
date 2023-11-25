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
        [HarmonyPostfix]
        [HarmonyPatch(nameof(WorldHandler.CharacterIsAlreadyNPCInScene))]
        private static void CharacterIsAlreadyNPCInScene_Postfix(ref bool __result, WorldHandler __instance, Characters c) {
            var baseCharacter = WinterCharacters.GetBaseCharacter(c);
            if (baseCharacter == Characters.NONE)
                return;
            foreach (var npc in __instance.sceneObjectsRegister.NPCs) {
                if (npc.character == baseCharacter && npc.isActive && npc.dialogueLevel <= npc.highestDialogLevel) {
                    __result = true;
                    return;
                }
            }
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
