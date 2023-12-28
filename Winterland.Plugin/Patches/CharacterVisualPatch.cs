using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Reptile;
using Winterland.Common;
using UnityEngine;
using System.Linq.Expressions;
using UnityEngine.TextCore.Text;

namespace Winterland.Plugin.Patches {
    [HarmonyPatch(typeof(CharacterVisual))]
    internal class CharacterVisualPatch {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(CharacterVisual.Init))]
        private static void Init_Postfix(Characters character, RuntimeAnimatorController animatorController, CharacterVisual __instance) {
            OverrideAnimations(__instance, character);
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(CharacterVisual.SetMoveStyleVisualAnim))]
        private static void SetMoveStyleVisualAnim_Postfix(Player player, CharacterVisual __instance) {
            var character = Characters.NONE;
            if (player != null)
                character = player.character;
            OverrideAnimations(__instance, character);
        }

        private static void OverrideAnimations(CharacterVisual visual, Characters character) {
            if (WinterAssets.Instance == null) return;
            var animatorOverride = visual.anim.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride == null) {
                animatorOverride = new AnimatorOverrideController(visual.anim.runtimeAnimatorController);
                visual.anim.runtimeAnimatorController = animatorOverride;
            }
            if (WinterCharacters.IsSanta(character)) {
                if (animatorOverride["softBounce13"] != WinterAssets.Instance.PlayerSantaBounce)
                    animatorOverride["softBounce13"] = WinterAssets.Instance.PlayerSantaBounce;
            }
            else if (animatorOverride["softBounce13"] == WinterAssets.Instance.PlayerSantaBounce) {
                animatorOverride["softBounce13"] = null;
            }
        }
    }
}
