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
    private static void UpdatePlugin() {
        var rootFolder = Path.GetDirectoryName(Directory.GetCurrentDirectory());
        var initializeScript = Path.Combine(rootFolder, "scripts", "rebuild.ps1");
        RunScript(initializeScript);
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
