using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using HarmonyLib;
using UnityEngine;

namespace MapStation.Plugin.Patches {
    [HarmonyPatch(typeof(CharacterVisual))]
    internal static class CharacterVisualPatch {
        // HACK - Fixes spraycan and phone textures glitching out when loading into custom stages with FasterLoadTimes enabled, for some reason.
        [HarmonyPostfix]
        [HarmonyPatch(nameof(CharacterVisual.InitVFX))]
        private static void InitVFX_Postfix(CharacterVisual __instance) {
            if (__instance.VFX == null) return;
            var assets = Core.Instance.Assets;
            var sprayCanTex = assets.LoadAssetFromBundle<Texture2D>("common_assets", "spraycanTex");
            if (__instance.VFX.spraycan != null)
                __instance.VFX.spraycan.GetComponent<Renderer>().sharedMaterial.mainTexture = sprayCanTex;
            var flipPhoneTex = assets.LoadAssetFromBundle<Texture2D>("common_assets", "flipPhoneTex");
            if (__instance.VFX.phone != null)
                __instance.VFX.phone.GetComponent<Renderer>().sharedMaterial.mainTexture = flipPhoneTex;
        }
    }
}
