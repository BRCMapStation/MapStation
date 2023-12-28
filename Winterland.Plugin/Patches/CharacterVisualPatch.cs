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
            if (!WinterCharacters.IsSanta(character)) return;
            OverrideAnimations(__instance);
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(CharacterVisual.SetMoveStyleVisualAnim))]
        private static void SetMoveStyleVisualAnim_Postfix(Player player, CharacterVisual __instance) {
            var character = player.character;
            if (!WinterCharacters.IsSanta(character)) return;
            OverrideAnimations(__instance);
        }

        private static void OverrideAnimations(CharacterVisual visual) {
            if (WinterAssets.Instance == null) return;
            var animatorOverride = visual.anim.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride == null) {
                animatorOverride = new AnimatorOverrideController(visual.anim.runtimeAnimatorController);
                visual.anim.runtimeAnimatorController = animatorOverride;
            }
            animatorOverride["softBounce13"] = WinterAssets.Instance.PlayerSantaBounce;
        }
    }
}
