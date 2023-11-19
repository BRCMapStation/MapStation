using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;

public class BuildAssets {

    private const string OutputDirectory = "AssetBundles";
    private const string PluginName = "MilleniumWinterland";

    [MenuItem("BRC/Build Assets _F5", priority = -50)]
    private static void BuildAllAssetBundles() {
        if (PluginEditor.IsPluginOutOfDate()) {
            UnityEngine.Debug.Log("Winterland assemblies seem to be out of date, rebuilding!");
            var rebuildProcess = PluginEditor.RebuildPlugin();
            rebuildProcess.WaitForExit();
        }
        PreBuildAssetBundles();
        CleanUpOutputDirectoryPreBuild(OutputDirectory);
        BuildAssetBundles(OutputDirectory);
        CleanUpOutputDirectoryPostBuild(OutputDirectory);
        CopyToBepInExPluginsFolder(OutputDirectory, PluginName);
        UnityEngine.Debug.Log("Done building assets!");
    }

    private static void CopyToBepInExPluginsFolder(string outputDirectory, string pluginName) {
        var bepinexDirectory = Environment.GetEnvironmentVariable("BepInExDirectory", EnvironmentVariableTarget.User);
        if (bepinexDirectory == null)
            bepinexDirectory = Environment.GetEnvironmentVariable("BepInExDirectory", EnvironmentVariableTarget.Machine);
        if (bepinexDirectory == null) {
            UnityEngine.Debug.LogWarning($"Please set your BepInExDirectory Environment variable on your system to your BepInEx path for the asset bundles to be automatically copied to the plugin folder.");
            return;
        }
        var pathToPlugin = Path.Combine(bepinexDirectory, "plugins", pluginName);
        var pathToBundles = Path.Combine(pathToPlugin, outputDirectory);
        if (Directory.Exists(pathToBundles))
            Directory.Delete(pathToBundles, true);
        if (!Directory.Exists(pathToBundles))
            Directory.CreateDirectory(pathToBundles);
        CopyDirectory(outputDirectory, pathToBundles, true);
    }

    // https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
    private static void CopyDirectory(string sourceDir, string destinationDir, bool recursive) {
        // Get information about the source directory
        var dir = new DirectoryInfo(sourceDir);

        // Check if the source directory exists
        if (!dir.Exists)
            throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

        // Cache directories before we start copying
        var dirs = dir.GetDirectories();

        // Create the destination directory
        Directory.CreateDirectory(destinationDir);

        // Get the files in the source directory and copy to the destination directory
        foreach (var file in dir.GetFiles()) {
            var targetFilePath = Path.Combine(destinationDir, file.Name);
            file.CopyTo(targetFilePath);
        }

        // If recursive and copying subdirectories, recursively call this method
        if (recursive) {
            foreach (var subDir in dirs) {
                var newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                CopyDirectory(subDir.FullName, newDestinationDir, true);
            }
        }
    }

    private static void PreBuildAssetBundles() {
        AssetDatabase.RemoveUnusedAssetBundleNames();
        var stageScenes = Directory.GetFiles("Assets/Stage Additions", "*.prefab", SearchOption.AllDirectories);
        foreach(var file in stageScenes) {
            var assetImporter = AssetImporter.GetAtPath(file);
            assetImporter.assetBundleName = $"Stages/{Path.GetFileNameWithoutExtension(file)}";
        }
    }

    private static void CleanUpOutputDirectoryPreBuild(string directory) {
        if (Directory.Exists(directory))
            Directory.Delete(directory, true);
    }

    private static void CleanUpOutputDirectoryPostBuild(string directory) {
        var directoryName = Path.GetFileName(directory);
        var filePath = Path.Combine(directory, directoryName);
        if (File.Exists(filePath))
            File.Delete(filePath);
        var manifestFiles = Directory.GetFiles(directory, "*.manifest", SearchOption.AllDirectories);
        foreach(var file in manifestFiles) {
            File.Delete(file);
        }
    }

    private static void BuildAssetBundles(string directory) {
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}
