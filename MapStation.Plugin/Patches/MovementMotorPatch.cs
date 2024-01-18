using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using MapStation.Plugin.Gameplay;
using HarmonyLib;
using UnityEngine;
using ECM.Common;

namespace MapStation.Plugin.Patches {
    [HarmonyPatch(typeof(MovementMotor))]
    internal static class MovementMotorPatch {
        /*
        [HarmonyPrefix]
        [HarmonyPatch(nameof(MovementMotor.ApplyGroundMovement))]
        private static void ApplyGroundMovement_Prefix(MovementMotor __instance, ref Vector3 desiredVelocity) {
            var player = __instance.gameObject.GetComponent<Player>();
            if (player == null) return;
            var mpPlayer = MapStationPlayer.Get(player);
            var normal = player.motor.groundNormal;
            if (mpPlayer.OnVertGround && Vector3.Angle(normal, Vector3.up) >= MapStationPlayer.MinimumVertGravityAngle && player.GetVelocity().magnitude < MapStationPlayer.VertMinimumSpeed) {
                var targetVector = (Vector3.down - Vector3.Project(Vector3.down, normal)).normalized;
                var targetRotation = Quaternion.LookRotation(targetVector, normal);
                var currentRotation = Quaternion.Lerp(__instance.rotation, targetRotation, MapStationPlayer.VertGravityTurnSpeed * Core.dt);
                player.SetRotation(currentRotation);
                var currentForward = currentRotation * Vector3.forward;
                var downDot = Mathf.Max(0f, Vector3.Dot(currentForward, targetVector));
                var downAcceleration = downDot * MapStationPlayer.VertGravity;
                //desiredVelocity = currentForward * downAcceleration;
                __instance.velocity += currentForward * (downAcceleration * Core.dt);
                //player.moveInput = desiredVelocity.normalized;
                player.targetMovement = Player.MovementType.WALKING;
                //player.SetForwardSpeed(downAcceleration);
            }
        }*/

        [HarmonyPrefix]
        [HarmonyPatch(nameof(MovementMotor.ApplyGroundMovement))]
        private static void ApplyGroundMovement_Prefix(MovementMotor __instance, out GroundHit __state) {
            var prevGroundHit = __instance.groundDetection.prevGroundHit;
            __state = prevGroundHit;
            var player = __instance.gameObject.GetComponent<Player>();
            if (player == null) return;
            var mpPlayer = MapStationPlayer.Get(player);
            if (mpPlayer.OnVertGround && !prevGroundHit.isOnGround && !prevGroundHit.isValidGround) {
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
