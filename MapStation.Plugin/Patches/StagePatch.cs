using System;
using HarmonyLib;
using Reptile;

namespace MapStation.Plugin.Patches;

[HarmonyPatch(typeof(Enum))]
internal static class StagePatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Enum.ToString))]
    [HarmonyPatch(new Type[] {})]
    private static void ToString_Postfix(ref string __result, Enum __instance) {
        if(__instance is Stage s) {
            if(StageEnum.IsValidMapId(s)) {
                if(StageEnum.MapNames.TryGetValue(s, out var name)) {
                    __result = StageEnum.MapNamePrefix + name;
                } else {
                    __result = StageEnum.MapNamePrefix + StageEnum.MapNameUnknownInfix + __result;
                }
            }
        }
    }
}