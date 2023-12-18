using BepInEx.Logging;
using UnityEngine;

namespace Winterland.Common;

public static class WinterLogging {
#if WINTER_DEBUG
    private const bool IsDebugBuild = true;
#else
    private const bool IsDebugBuild = false;
#endif
    public static ManualLogSource CreateLogger(string name = null, bool onlyForDebugBuild = false) {
        var loggerName = name == null ? "Winterland" : $"Winterland {name}";
        var logger = new ManualLogSource(loggerName);
        if (!onlyForDebugBuild || IsDebugBuild) {
            BepInEx.Logging.Logger.Sources.Add(logger);
        }
        return logger;
    }
}
