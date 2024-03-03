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
using UnityEngine.UIElements;

namespace MapStation.Plugin.Patches {
    [HarmonyPatch(typeof(GroundDetection))]
    internal static class GroundDetectionPatch {
        private static void ComputeVert(GroundDetection __instance) {
            var player = __instance.player;
            var mpPlayer = MapStationPlayer.Get(player);
            mpPlayer.HasVertBelow = false;
            var dist = 500f;

            var vertRay = __instance.GetRaycastInfo(player.transform.position + (Vector3.up * 1f), Vector3.down, dist, 1f);
            
            if (!vertRay.hit) return;

            var vertAngle = Vector3.Angle(Vector3.up, vertRay.hitInfo.normal);
            var vert = vertRay.hitInfo.collider.GetComponent<MapStationVert>();

            if (vert == null) return;

            if (vertAngle >= MapStationPlayer.MinimumAirVertAngle) {
                mpPlayer.HasVertBelow = true;
                if (!mpPlayer.OnVertGround)
                    mpPlayer.GroundVertVector = -vertRay.hitInfo.normal;
            }

            if (!mpPlayer.OnVertAir) return;

            var nudgeRay = __instance.GetRaycastInfo(player.transform.position - (mpPlayer.AirVertVector * MapStationPlayer.VertOuterRay) + (Vector3.up * 1f), Vector3.down, dist, 1f);
            if (nudgeRay.hit) {
                var nudgeVert = nudgeRay.hitInfo.collider.GetComponent<MapStationVert>();
                if (nudgeVert == null || Vector3.Angle(nudgeRay.hitInfo.normal, Vector3.up) < MapStationPlayer.MinimumAirVertAngle) {
                    player.transform.position += mpPlayer.AirVertVector * MapStationPlayer.VertOuterNudge;
                }
            }

            nudgeRay = __instance.GetRaycastInfo(player.transform.position + (mpPlayer.AirVertVector * MapStationPlayer.VertInnerRay) + (Vector3.up * 1f), Vector3.down, dist, 1f);
            if (nudgeRay.hit) {
                var nudgeVert = nudgeRay.hitInfo.collider.GetComponent<MapStationVert>();
                if (nudgeVert == null || Vector3.Angle(nudgeRay.hitInfo.normal, Vector3.up) < MapStationPlayer.MinimumAirVertAngle) {
                    player.transform.position -= mpPlayer.AirVertVector * MapStationPlayer.VertInnerNudge;
                }
            }

            if (vertAngle < MapStationPlayer.MinimumAirVertAngle) return;

            var vertVector = MapStationPlayer.GetVertVectorFromGroundNormal(vertRay.hitInfo.normal);
            if (vertVector == mpPlayer.AirVertVector) return;

            mpPlayer.TransferVert(vertVector);
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GroundDetection.ComputeGroundHit))]
        private static bool ComputeGroundHit_Prefix(GroundDetection __instance, ref bool __result, Vector3 position, Quaternion rotation, ref GroundHit groundHitInfo, float distance) {
            var mpPlayer = MapStationPlayer.Get(__instance.player);

            if (!mpPlayer.MoveStyleEquipped) {
                mpPlayer.OnVertGround = false;
                mpPlayer.WasOnVertGround = false;
                mpPlayer.HasVertBelow = false;
                return true;
            }

            ComputeVert(__instance);

            mpPlayer.OnVertGround = false;

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
                dist = distance * 1.25f;
                direction = -mpPlayer.AirVertVector;
            }

            var rayVert = __instance.GetRaycastInfo(position, direction, dist, 1f);
            /*
            if (!rayVert.hit && mpPlayer.OnVertAir) {
                var extraRayDirection = Vector3.down;
                var extraRayDistance = 2f;
                rayVert = __instance.GetRaycastInfo(position, extraRayDirection, extraRayDistance, 1f);
            }*/

            if (!rayVert.hit) {
                var hitSphere = __instance.BottomSphereCast(position, rotation, out var hitInfo, distance);
                if (!hitSphere) return true;
                rayVert.hit = true;
                rayVert.hitInfo = hitInfo;
            }
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

            if (!__result && (mpPlayer.WasOnVertGround || mpPlayer.OnVertGround || mpPlayer.HasVertBelow) && __instance.prevGroundHit.isValidGround && Vector3.Angle(-mpPlayer.GroundVertVector, Vector3.up) >= MapStationPlayer.MinimumAirVertAngle && __instance.player.motor.velocity.y > 0f) {
                mpPlayer.AirVertBegin();
            }

            if (__result && mpPlayer.OnVertAir && groundHitInfo.isValidGround) {
                mpPlayer.AirVertEnd();
            }

            if (mpPlayer.OnVertGround)
                mpPlayer.GroundVertVector = -groundHitInfo.groundNormal;
            
            if (!mpPlayer.OnVertGround && groundHitInfo.isValidGround && groundHitInfo.isOnGround && mpPlayer.MoveStyleEquipped && mpPlayer.GroundVertVector != Vector3.down) {
                var mpVert = groundHitInfo.groundCollider.GetComponent<MapStationVert>();
                if (mpVert != null) {
                    mpPlayer.OnVertGround = true;
                }
            }

            if (!mpPlayer.OnVertGround)
                mpPlayer.GroundVertVector = Vector3.down;

            mpPlayer.WasOnVertGround = mpPlayer.OnVertGround;
        }
    }
}
