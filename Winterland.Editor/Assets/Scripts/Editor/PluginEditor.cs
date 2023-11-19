using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public class PluginEditor : MonoBehaviour
{
    [MenuItem("BRC/Initialize Plugin", priority = 0)]
    private static void InitializePlugin() {
        var rootFolder = Path.GetDirectoryName(Directory.GetCurrentDirectory());
        var initializeScript = Path.Combine(rootFolder, "initialize.ps1");
        RunScript(initializeScript);
    }

    [MenuItem("BRC/Initialize Plugin", true, priority = 0)]
    private static bool InitializePluginValidate() {
        return !Plugininitialized();
    }

    [MenuItem("BRC/Update Plugin _F6", priority = 1)]
    private static void UpdatePlugin() {
        var rootFolder = Path.GetDirectoryName(Directory.GetCurrentDirectory());
        var initializeScript = Path.Combine(rootFolder, "scripts", "rebuild.ps1");
        RunScript(initializeScript);
    }

    [MenuItem("BRC/Update Plugin _F6", true, priority = 1)]
    private static bool UpdatePluginValidate() {
        return Plugininitialized();
    }

    private static bool Plugininitialized() {
        var dir = Environment.GetEnvironmentVariable("BepInExDirectory", EnvironmentVariableTarget.User);
        if (dir == null)
            dir = Environment.GetEnvironmentVariable("BepInExDirectory", EnvironmentVariableTarget.Machine);
        if (dir == null)
            return false;

        dir = Environment.GetEnvironmentVariable("BRCPath", EnvironmentVariableTarget.User);
        if (dir == null)
            dir = Environment.GetEnvironmentVariable("BRCPath", EnvironmentVariableTarget.Machine);
        if (dir == null)
            return false;

        if (!File.Exists(Path.Combine("Assets/Scripts", "Winterland.Common.dll")))
            return false;

        return true;
    }

    private static void RunScript(string script) {
        var startInfo = new ProcessStartInfo();
        startInfo.FileName = @"C:\windows\system32\windowspowershell\v1.0\powershell.exe";
        startInfo.WorkingDirectory = Path.GetDirectoryName(script);
        script = "\"&'" + script + "'\"";
        startInfo.Arguments = script;
        Process.Start(startInfo);
    }
}
