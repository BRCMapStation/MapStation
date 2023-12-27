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

namespace Winterland.Plugin.Patches {
    [HarmonyPatch(typeof(CharacterVisual))]
    internal class CharacterVisualPatch {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(CharacterVisual.Init))]
        private static void Init_Postfix(Characters character, RuntimeAnimatorController animatorController, CharacterVisual __instance) {
            if (!WinterCharacters.IsSanta(character)) return;
            if (WinterAssets.Instance == null) return;
            var visual = __instance;
            var animatorOverride = new AnimatorOverrideController(animatorController);
            visual.anim.runtimeAnimatorController = animatorOverride;
            animatorOverride["softBounce13"] = WinterAssets.Instance.PlayerSantaBounce;
        }
    }
}
