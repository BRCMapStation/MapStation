using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using HarmonyLib;
using MapStation.Plugin.Gameplay;
using System.Reflection.Emit;
using System.Reflection;
using UnityEngine;

namespace MapStation.Plugin.Patches {
    [HarmonyPatch(typeof(GrindAbility))]
    internal static class GrindAbilityPatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(GrindAbility.SetToLine))]
        private static void SetToLine_Prefix(GrindAbility __instance) {
            var player = __instance.p;
            var mpPlayer = MapStationPlayer.Get(player);
            if (mpPlayer.OnVertGround || mpPlayer.OnVertAir || mpPlayer.HasVertBelow) {
                player.OrientVisualInstantReset();
            }
        }
    }
}

[HarmonyPatch(typeof(GrindAbility))]
[HarmonyPatch(nameof(GrindAbility.FixedUpdateAbility))]
public static class GrindAbilityMovingHandplantPatch
{
    private static MethodInfo Mi_HandplantAbility_SetToPole_1 = SymbolExtensions.GetMethodInfo(() => ((HandplantAbility)null).SetToPole(Vector3.one));
    private static MethodInfo Mi_HandplantAbility_SetToPole_2 = SymbolExtensions.GetMethodInfo(() => ((HandplantAbility)null).SetToPole(Vector3.one, (SkateboardScrewPole)null));
    private static MethodInfo Mi_GrindNode_get_position = typeof(GrindNode).GetMethod("get_position");
    private static MethodInfo Mi_HandplantAbility_SetToPole_1_Replacement = SymbolExtensions.GetMethodInfo(() => HandplantAbility_SetToPole_1_Replacement((HandplantAbility)null, (GrindNode)null));
	static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
	{
		var codes = new List<CodeInstruction>(instructions);
        for (var i = 1; i < codes.Count; i++)
        {
            var code = codes[i];
            if(code.Calls(Mi_HandplantAbility_SetToPole_1)) {
                var previousCode = codes[i - 1];
                if(previousCode.Calls(Mi_GrindNode_get_position)) {
                    codes[i - 1] = new CodeInstruction(OpCodes.Nop);
                    codes[i] = new CodeInstruction(OpCodes.Call, Mi_HandplantAbility_SetToPole_1_Replacement);
                }
            }
        }
        return codes;
	}

    static void HandplantAbility_SetToPole_1_Replacement(HandplantAbility __instance, GrindNode plantOnNode) {
        var player = __instance.p;
        var mpPlayer = MapStationPlayer.Get(player);
        var mpAbility = mpPlayer.MapStationHandplantAbility;
        mpAbility.SetToPole(plantOnNode.transform);
    }
}
