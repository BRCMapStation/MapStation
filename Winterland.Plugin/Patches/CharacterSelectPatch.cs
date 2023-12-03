using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Winterland.Common;
using Reptile;

namespace Winterland.Plugin.Patches {
    [HarmonyPatch(typeof(CharacterSelect))]
    internal class CharacterSelectPatch {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(CharacterSelect.PopulateListOfSelectableCharacters))]
        private static void PopulateListOfSelectableCharacters_Postfix(CharacterSelect __instance, Player player) {
            var newList = new List<Characters>();
            foreach(var character in __instance.selectableCharacters) {
                var baseCharacter = WinterCharacters.GetBaseCharacter(character);
                if (baseCharacter == Characters.NONE) {
                    newList.Add(character);
                    continue;
                }

                var playerBaseCharacter = WinterCharacters.GetBaseCharacter(player.character);
                if (playerBaseCharacter == Characters.NONE)
                    playerBaseCharacter = player.character;

                if (baseCharacter == playerBaseCharacter) {
                    newList.Add(character);
                    continue;
                }

                if (__instance.IsCharacterUnlocked(baseCharacter)) {
                    newList.Add(character);
                    continue;
                }
            }
            __instance.selectableCharacters = newList;
        }
    }
}
