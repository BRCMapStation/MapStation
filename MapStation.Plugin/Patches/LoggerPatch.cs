using BepInEx.Logging;
using HarmonyLib;

namespace MapStation.Plugin.Patches;

[HarmonyPatch(typeof(Logger))]
class LoggerPatch {
    private static readonly object Lock = new object();

    [HarmonyPrefix]
    [HarmonyPatch("InternalLogEvent")]
    private static bool InternalLogEvent_Prefix(object sender, LogEventArgs eventArgs)
    {
        lock(Lock) {
            // Copy-paste of BepInEx source: https://github.com/BepInEx/BepInEx/blob/0d06996b52c0215a8327b8c69a747f425bbb0023/BepInEx/Logging/Logger.cs#L35C3-L41C4
			foreach (var listener in Logger.Listeners)
			{
				listener?.LogEvent(sender, eventArgs);
			}
		}
        return false;
    }
}