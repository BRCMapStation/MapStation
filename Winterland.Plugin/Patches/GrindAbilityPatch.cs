using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using HarmonyLib;
using UnityEngine;

namespace Winterland.Plugin.Patches {
    /// <summary>
    /// Fixes a vanilla bug that can sometimes get you stuck at the end of grind lines. Was a bit of a problem in Winterland.
    /// </summary>
    [HarmonyPatch(typeof(GrindAbility))]
    internal class GrindAbilityPatch {
        private static bool Fixed = false;
        private static Vector3 DeltaPosition = Vector3.zero;
        private static Quaternion PreviousRotation = Quaternion.identity;
        [HarmonyPrefix]
        [HarmonyPatch(nameof(GrindAbility.FixedUpdateAbility))]
        private static void FixedUpdateAbility_Prefix(GrindAbility __instance) {
            var epsilon = float.Epsilon;
            var playerPos = __instance.p.tf.position;
            var playerForward = __instance.p.tf.forward;
            var nextNode = __instance.grindLine.GetNextNode(playerForward);
            var viewVector = (nextNode - playerPos).normalized;
            if (viewVector.sqrMagnitude <= epsilon) {
                Fixed = true;
                DeltaPosition = 1f * (PreviousRotation * Vector3.forward);
                __instance.p.SetRotHard(PreviousRotation);
                __instance.p.tf.position += DeltaPosition;
            }
            PreviousRotation = __instance.p.tf.rotation;
        }
        [HarmonyPostfix]
        [HarmonyPatch(nameof(GrindAbility.FixedUpdateAbility))]
        private static void FixedUpdateAbility_Postfix(GrindAbility __instance) {
            if (Fixed) {
                __instance.p.tf.position -= DeltaPosition;
            }
            Fixed = false;
        }
    }
}
