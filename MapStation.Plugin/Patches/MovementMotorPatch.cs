using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using MapStation.Plugin.Gameplay;
using HarmonyLib;

namespace MapStation.Plugin.Patches {
    [HarmonyPatch(typeof(MovementMotor))]
    internal static class MovementMotorPatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(MovementMotor.ApplyGroundMovement))]
        private static void ApplyGroundMovement_Prefix(MovementMotor __instance, out GroundHit __state) {
            var prevGroundHit = __instance.groundDetection.prevGroundHit;
            __state = prevGroundHit;
            var player = __instance.gameObject.GetComponent<Player>();
            if (player == null) return;
            var mpPlayer = MapStationPlayer.Get(player);
            if (mpPlayer.OnVertGround) {
                prevGroundHit.isOnGround = true;
                prevGroundHit.isValidGround = true;
                __instance.groundDetection.prevGroundHit = prevGroundHit;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(MovementMotor.ApplyGroundMovement))]
        private static void ApplyGroundMovement_Postfix(MovementMotor __instance, GroundHit __state) {
            __instance.groundDetection.prevGroundHit = __state;
        }
    }
}
