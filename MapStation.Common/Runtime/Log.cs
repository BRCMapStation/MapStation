#if BEPINEX
using BepInEx.Logging;
#endif
using UnityEngine;

namespace MapStation.Common {
    /// <summary>
    /// Static aliases to either the plugin's logger in BepInEx, or Unity's logger in the editor.
    /// Makes logging code terser.
    /// </summary>
    public static class Log {
#if BEPINEX
        public static ManualLogSource Logger;
        public static void Info(string message) {
            Logger.LogInfo(message);
        }
        public static void Error(string message) {
            Logger.LogError(message);
        }
        public static void Warning(string message) {
            Logger.LogWarning(message);
        }
        public static void Debug(string message) {
            Logger.LogDebug(message);
        }
#else
        public static void Info(string message) {
            UnityEngine.Debug.Log(message);
        }
        public static void Error(string message) {
            UnityEngine.Debug.LogError(message);
        }
        public static void Warning(string message) {
            UnityEngine.Debug.LogWarning(message);
        }
        public static void Debug(string message) {
            UnityEngine.Debug.Log(message);
        }
#endif
    }
}
