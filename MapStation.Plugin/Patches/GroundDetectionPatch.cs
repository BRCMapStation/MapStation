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

            var dist = distance;

            if (mpPlayer.WasOnVertGround)
                dist *= 2f;

            var rayVert = __instance.GetRaycastInfo(position, mpPlayer.GroundVertVector, dist, 1f);

            if (!rayVert.hit) return true;
            if (Vector3.Angle(rayVert.hitInfo.normal, Vector3.up) < MapStationPlayer.MinimumGroundVertAngle) return true;

            var vert = rayVert.hitInfo.collider.GetComponent<MapStationVert>();

            if (vert == null) return true;

            groundHitInfo.SetFrom(rayVert.hitInfo);
            groundHitInfo.groundDistance = 0f;
            groundHitInfo.isOnGround = true;
            groundHitInfo.isValidGround = true;
            groundHitInfo.groundNormalVisual = groundHitInfo.groundNormal;
            mpPlayer.OnVertGround = true;

            __result = true;
            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(GroundDetection.ComputeGroundHit))]
        private static void ComputeGroundHit_Postfix(GroundDetection __instance, ref bool __result, Vector3 position, Quaternion rotation, ref GroundHit groundHitInfo, float distance) {
            var mpPlayer = MapStationPlayer.Get(__instance.player);

            if (mpPlayer.OnVertGround)
                mpPlayer.GroundVertVector = -groundHitInfo.groundNormal;
            else
                mpPlayer.GroundVertVector = Vector3.down;

            if (!__result && mpPlayer.WasOnVertGround) {
                Debug.Log("LEAVING VERT");
            }

            mpPlayer.WasOnVertGround = mpPlayer.OnVertGround;
        }
    }
}
