using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

/// <summary>
/// Hide pesky log messages from BepInEx console
/// </summary>
[HarmonyPatch(typeof(UnityLogSource))]
class Patch {
    [HarmonyPrefix]
    [HarmonyPatch("OnUnityLogMessageReceived")]
    private static bool OnUnityLogMessageReceived_Prefix(string message, string stackTrace, LogType type) {
        if(type == LogType.Warning && message.Contains("does not support negative scale or size")) {
            return false;
        }
        return true;
    }
}

// [HarmonyPatch(typeof(Debug))]
// class DebugPatch {
//     [HarmonyPrefix]
//     [HarmonyPatch(nameof(Debug.LogWarning), typeof(object))]
//     private static bool LogWarning_Prefix(object message) {
//         Log.Info("intercepted a warning 1");
//         return false;
//     }

//     [HarmonyPrefix]
//     [HarmonyPatch(nameof(Debug.LogWarning), typeof(object), typeof(Object))]
//     public static bool LogWarning(object message, Object context) {
//         Debug.Log("intercepted a warning 2");
//         return false;
//     }

//     [HarmonyPrefix]
//     [HarmonyPatch(nameof(Debug.LogWarningFormat), typeof(string), typeof(object[]))]
//     public static bool LogWarningFormat(string format, params object[] args)
//     {
//         Debug.Log("intercepted a warning 3");
//         return false;
//     }

//     [HarmonyPrefix]
//     [HarmonyPatch(nameof(Debug.LogWarningFormat), typeof(Object), typeof(string), typeof(object[]))]
//     public static bool LogWarningFormat(Object context, string format, params object[] args) {
//         Debug.Log("intercepted a warning 4");
//         return false;
//     }
// }
