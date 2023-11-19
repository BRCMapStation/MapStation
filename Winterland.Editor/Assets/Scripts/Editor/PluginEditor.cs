using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public class PluginEditor : MonoBehaviour
{
    [MenuItem("BRC/Update Plugin _F6", priority = 1)]
    private static void UpdatePlugin()
    {
        RebuildPlugin();
    }

    public static bool IsPluginOutOfDate()
    {
        var rootPath = Path.GetDirectoryName(Directory.GetCurrentDirectory());
        var commonAssemblyPath = Path.Combine("Assets/Scripts", "Winterland.Common.dll");
        if (!File.Exists(commonAssemblyPath))
            return true;
        var assemblyInfo = new FileInfo(commonAssemblyPath);
        var assemblyModifiedDate = assemblyInfo.LastWriteTime;
        var commonPath = Path.Combine(rootPath, "Winterland.Common");
        var latestModifiedCommon = GetLatestModifiedDateTimeForScriptsInDirectory(commonPath);
        if (latestModifiedCommon > assemblyModifiedDate)
            return true;
        return false;
    }

    private static DateTime GetLatestModifiedDateTimeForScriptsInDirectory(string directory)
    {
        var newestDate = DateTime.MinValue;
        var allScripts = Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories);
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
        var initializeScript = Path.Combine(rootFolder, "scripts", "rebuild.ps1");
        return RunScript(initializeScript);
    }

    private static Process RunScript(string script) {
        var startInfo = new ProcessStartInfo();
        startInfo.FileName = @"C:\windows\system32\windowspowershell\v1.0\powershell.exe";
        startInfo.WorkingDirectory = Path.GetDirectoryName(script);
        script = "\"&'" + script + "'\"";
        startInfo.Arguments = script;
        return Process.Start(startInfo);
    }
}
