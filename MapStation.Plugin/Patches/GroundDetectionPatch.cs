using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Reptile;
using MapStation.Common.Gameplay;
using UnityEngine;
using MapStation.Plugin.Gameplay;

namespace MapStation.Plugin.Patches {
    [HarmonyPatch(typeof(GroundDetection))]
    internal static class GroundDetectionPatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(GroundDetection.ComputeGroundHit))]
        private static bool ComputeGroundHit_Prefix(GroundDetection __instance, ref bool __result, Vector3 position, Quaternion rotation, ref GroundHit groundHitInfo, float distance) {
            var mpPlayer = MapStationPlayer.Get(__instance.player);

            mpPlayer.OnVertGround = false;

            if (!mpPlayer.MoveStyleEquipped) {
                mpPlayer.OnVertGround = false;
                mpPlayer.WasOnVertGround = false;
                return true;
            }

            if (mpPlayer.OnVertAir && __instance.player.motor.velocity.y > 0f) {
                __result = false;
                return false;
            }


            var direction = mpPlayer.GroundVertVector;

            var dist = distance;

            if (mpPlayer.WasOnVertGround || __instance.player.motor.wasGrounded)
                dist = distance * 2f;
            /*
            if (mpPlayer.OnVertAir) {
                dist = distance * 1.5f;
                direction = Vector3.down;
            }*/

            if (mpPlayer.OnVertAir) {
                dist = distance * 1.5f;
                direction = -mpPlayer.AirVertVector;
            }

            var rayVert = __instance.GetRaycastInfo(position, direction, dist, 1f);
            /*
            if (!rayVert.hit && mpPlayer.OnVertAir) {
                var extraRayDirection = Vector3.down;
                var extraRayDistance = 2f;
                rayVert = __instance.GetRaycastInfo(position, extraRayDirection, extraRayDistance, 1f);
            }*/

            if (!rayVert.hit) return true;
            //if (Vector3.Angle(rayVert.hitInfo.normal, Vector3.up) < MapStationPlayer.MinimumGroundVertAngle) return true;

            var vert = rayVert.hitInfo.collider.GetComponent<MapStationVert>();

            if (vert == null) return true;

            groundHitInfo.SetFrom(rayVert.hitInfo);
            groundHitInfo.groundDistance = 0.001f;
            groundHitInfo.isOnGround = true;
            groundHitInfo.isValidGround = true;
            groundHitInfo.groundNormal = groundHitInfo.groundNormal.normalized;
            groundHitInfo.groundNormalVisual = groundHitInfo.groundNormal;
            groundHitInfo.isOnStep = false;
            mpPlayer.VertCollider = groundHitInfo.groundCollider;
            mpPlayer.OnVertGround = true;

            __result = true;
            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(GroundDetection.ComputeGroundHit))]
        private static void ComputeGroundHit_Postfix(GroundDetection __instance, ref bool __result, Vector3 position, Quaternion rotation, ref GroundHit groundHitInfo, float distance) {
            var mpPlayer = MapStationPlayer.Get(__instance.player);

            if (!__result && mpPlayer.WasOnVertGround && Vector3.Angle(-mpPlayer.GroundVertVector, Vector3.up) >= MapStationPlayer.MinimumAirVertAngle) {
                mpPlayer.AirVertBegin();
            }

            if (__result && mpPlayer.OnVertAir && groundHitInfo.isValidGround) {
                mpPlayer.AirVertEnd();
            }

            if (mpPlayer.OnVertGround)
                mpPlayer.GroundVertVector = -groundHitInfo.groundNormal;
            else
                mpPlayer.GroundVertVector = Vector3.down;

            mpPlayer.WasOnVertGround = mpPlayer.OnVertGround;
        }
    }
}
