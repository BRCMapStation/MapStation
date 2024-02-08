#if MAPSTATION_DEBUG
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using MapStation.Tools.DevTools;
using UnityEditor;
using UnityEngine;

public class PluginEditor : MonoBehaviour
{
    [MenuItem(UIConstants.menuLabel + "/Update Plugin _F7", priority = (int)UIConstants.MenuOrder.UPDATE_PLUGIN)]
    private static void UpdatePlugin()
    {
        RebuildPlugin();
    }

    public static bool IsPluginOutOfDate()
    {
        var rootPath = Path.GetDirectoryName(Directory.GetCurrentDirectory());
        var commonPath = Path.Combine(rootPath, "MapStation.Common");
        var pluginPath = Path.Combine(rootPath, "MapStation.Plugin");
        var binPath = Path.Combine(pluginPath, "bin");
        if (!Directory.Exists(binPath))
            return true;
        var latestDllModifiedDate = GetLatestModifiedDateTimeForFilesInDirectory(binPath, "*.dll");
        var latestCommonSourceCodeModifiedDate = GetLatestModifiedDateTimeForFilesInDirectory(commonPath, "*.cs");
        var latestPluginSourceCodeModifiedDate = GetLatestModifiedDateTimeForFilesInDirectory(pluginPath, "*.cs");
        if (latestCommonSourceCodeModifiedDate > latestDllModifiedDate || latestPluginSourceCodeModifiedDate > latestDllModifiedDate)
            return true;
        return false;
    }

    private static DateTime GetLatestModifiedDateTimeForFilesInDirectory(string directory, string pattern)
    {
        var newestDate = DateTime.MinValue;
        var allScripts = Directory.GetFiles(directory, pattern, SearchOption.AllDirectories);
        foreach(var script in allScripts)
        {
            var scriptInfo = new FileInfo(script);
            var scriptModifiedDate = scriptInfo.LastWriteTime;
            if (scriptModifiedDate > newestDate)
                newestDate = scriptModifiedDate;
        }
        return newestDate;
    }

    public static Process RebuildPlugin()
    {
        var rootFolder = Path.GetDirectoryName(Directory.GetCurrentDirectory());
        var rebuildScript = Path.Combine(rootFolder, "scripts", "rebuild.ps1");
        var rebuildProcess = PowershellUtil.RunScript(rebuildScript);
        rebuildProcess.WaitForExit();
        var copyAssetsScript = Path.Combine(rootFolder, "scripts", "copy-assets.ps1");
        var copyAssetsProcess = PowershellUtil.RunScript(copyAssetsScript);
        return copyAssetsProcess;
    }
}
#endif
