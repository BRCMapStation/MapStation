using Reptile;
using HarmonyLib;
using Winterland.Plugin;
using MapStation.Common.Gameplay;
using UnityEngine.UIElements;
using MapStation.Plugin.Gameplay;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace MapStation.Plugin.Patches;

[HarmonyPatch(typeof(Player))]
internal static class PlayerPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Player.Init))]
    private static void Init_Postfix(Player __instance) {
        if(MapStationConfig.Instance.DisableKBMInputValue) {
            KBMInputDisabler.Disable();
        }
        __instance.gameObject.AddComponent<MapStationPlayer>();
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Player.OnLanded))]
    private static bool OnLanded_Prefix(Player __instance) {
        var mpPlayer = MapStationPlayer.Get(__instance);
        if (mpPlayer.OnVertGround && Vector3.Angle(__instance.motor.groundNormal, Vector3.up) >= MapStationPlayer.MinimumGroundVertAngle)
            return false;
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Player.UpdateRotation))]
    private static bool UpdateRotation_Prefix(Player __instance) {
        var mpPlayer = MapStationPlayer.Get(__instance);
        if (mpPlayer.OnVertAir) return false;
        return true;
    }

    private struct OrientState {
        public Player.MovementType targetMovement;
        public MoveStyle moveStyle;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Player.OrientVisualToSurface))]
    private static bool OrientVisualToSurface_Prefix(Player __instance, out OrientState __state) {
        var mpPlayer = MapStationPlayer.Get(__instance);
        var orient = new OrientState() { targetMovement = __instance.targetMovement, moveStyle = __instance.moveStyle };
        __state = orient;
        if (mpPlayer.OnVertGround) {
            __instance.targetMovement = Player.MovementType.RUNNING;
            __instance.moveStyle = MoveStyle.SKATEBOARD;
        }
        if (mpPlayer.OnVertAir) {
            mpPlayer.UpdateVertRotation();
            return false;
        }
        return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Player.OrientVisualToSurface))]
    private static void OrientVisualToSurface_Postfix(Player __instance, OrientState __state) {
        __instance.targetMovement = __state.targetMovement;
        __instance.moveStyle = __state.moveStyle;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Player.SetMoveStyle))]
    private static void SetMoveStyle_Prefix(Player __instance, MoveStyle setMoveStyle) {
        if (setMoveStyle == MoveStyle.ON_FOOT) {
            var boostAbility = __instance.ability as BoostAbility;
            if (boostAbility != null)
                if (boostAbility.equippedMovestyleWasUsed) return;
            var mpPlayer = MapStationPlayer.Get(__instance);
            if (mpPlayer.OnVertAir)
                mpPlayer.AirVertEnd();
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Player.CompletelyStop))]
    private static void CompletelyStop_Postfix(Player __instance) {
        var mpPlayer = MapStationPlayer.Get(__instance);
        mpPlayer.SpeedFromVertAir = 0f;
        if (mpPlayer.OnVertAir)
            mpPlayer.AirVertEnd();
    }

    /*
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Player.CheckVert))]
    private static bool CheckVert(Player __instance, ref bool __result) {
        if (__instance.OnAnyGround() && __instance.motor.groundCollider.GetComponent<MapStationVert>() != null) {
            __instance.motor.groundDetection.groundLimit = 90f;
            __instance.OrientVisualInstant();
            UnityEngine.Debug.Log(__instance.motor.groundNormalVisual.ToString());
            //__result = true;
            return true;
        }
        __instance.motor.groundDetection.groundLimit = 60f;
        return true;
    }*/
}
