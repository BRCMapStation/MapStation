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
        if (mpPlayer.OnVertGround && Vector3.Angle(__instance.motor.groundNormal, Vector3.up) >= MapStationPlayer.MinimumGroundVertAngle) {
            __instance.OrientVisualInstant();
            __instance.audioManager.PlaySfxGameplay(__instance.moveStyle, AudioClipID.land, __instance.playerOneShotAudioSource, 0f);
            return false;
        }
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Player.UpdateRotation))]
    private static bool UpdateRotation_Prefix(Player __instance) {
        var mpPlayer = MapStationPlayer.Get(__instance);
        if (mpPlayer.OnVertAir) return false;
        return true;
    }

    private struct MoveState {
        public float airDecc;
        public float aboveMaxAirDecc;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Player.Move))]
    private static void Move_Prefix(Player __instance, out MoveState __state) {
        __state = new MoveState();
        __state.airDecc = __instance.stats.airDecc;
        __state.aboveMaxAirDecc = __instance.aboveMaxAirDecc;
        var mpPlayer = MapStationPlayer.Get(__instance);
        if (mpPlayer.OnVertAir) {
            __instance.stats.airDecc = 0f;
            __instance.aboveMaxAirDecc = 0f;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Player.Move))]
    private static void Move_Postfix(Player __instance, MoveState __state) {
        __instance.stats.airDecc = __state.airDecc;
        __instance.aboveMaxAirDecc = __state.aboveMaxAirDecc;
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

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Player.JumpIsAllowed))]
    private static bool JumpIsAllowed_Prefix(Player __instance, ref bool __result) {
        var mpPlayer = MapStationPlayer.Get(__instance);
        if (mpPlayer.OnVertGround && Vector3.Angle(__instance.motor.groundNormal, Vector3.up) >= MapStationPlayer.MinimumAirVertAngle) {
            __result = false;
            return false;
        }
        if (mpPlayer.OnVertAir) {
            __result = false;
            return false;
        }
        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Player.CanStartGrind))]
    private static bool CanStartGrind_Prefix(Player __instance, ref bool __result) {
        var player = __instance;
        if (player.jumpButtonHeld || player.jumpButtonNew || player.jumpRequested) return true;
        var mpPlayer = MapStationPlayer.Get(player);
        if (mpPlayer.OnVertAir) {
            __result = false;
            return false;
        }
        if (mpPlayer.HasVertBelow) {
            __result = false;
            return false;
        }
        if (mpPlayer.OnVertGround && Vector3.Angle(player.motor.groundNormal, Vector3.up) >= MapStationPlayer.MinimumAirVertAngle) {
            __result = false;
            return false;
        }
        return true;
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
