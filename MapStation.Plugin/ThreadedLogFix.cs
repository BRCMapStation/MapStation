using System.Reflection;
using System.Threading;
using BepInEx.Logging;
using UnityEngine;

namespace MapStation.Plugin;

class ThreadedLogFix {

    private static int MainThreadId;

    // Delegate to BepInEx's own event handler
    private delegate void UnityLogSource_OnUnityLogMessageReceived(string message, string stackTrace, LogType type);
    private static UnityLogSource_OnUnityLogMessageReceived UnityLogSource_OnUnityLogMessageReceived_Handler;

    public static void Install() {
        MainThreadId = Thread.CurrentThread.ManagedThreadId;
        UnityLogSource_OnUnityLogMessageReceived_Handler = (UnityLogSource_OnUnityLogMessageReceived)typeof(UnityLogSource).GetMethod("OnUnityLogMessageReceived", BindingFlags.NonPublic | BindingFlags.Static).CreateDelegate(typeof(UnityLogSource_OnUnityLogMessageReceived));
        Application.logMessageReceivedThreaded += onLogMessageReceivedThreaded;
    }

    private static void onLogMessageReceivedThreaded(string condition, string stackTrace, LogType type) {
        // BepInEx already listens to main thread logs, so only forward log messages if they're from other threads
        if(Thread.CurrentThread.ManagedThreadId != MainThreadId) {
            UnityLogSource_OnUnityLogMessageReceived_Handler(condition, stackTrace, type);
        }
    }
}