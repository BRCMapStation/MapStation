using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using MapStation.Common;
using MapStation.Tools;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MapBuilder {

    [MenuItem("BRC/Build Assets and Run on Steam _F6", priority = -49)]
    private static void BuildAndRunSteam() {
        BuildAllAssetBundles();
        GameLauncher.LaunchGameSteam();
    }

    [MenuItem("BRC/Build Assets and Run on Steam _F6", true, priority = -49)]
    private static bool BuildAndRunSteamValidate() {
        return (!GameLauncher.IsGameOpen() && GameLauncher.CanLaunchOnSteam());
    }
    [MenuItem("BRC/Build Assets _F5", priority = -50)]
    private static void BuildAllAssetBundles() {
#if MAPSTATION_DEBUG
        if (PluginEditor.IsPluginOutOfDate()) {
            UnityEngine.Debug.Log("MapStation assemblies seem to be out of date, rebuilding!");
            try {
                if (!GameLauncher.IsGameOpen())
                {
                    var rebuildProcess = PluginEditor.RebuildPlugin();
                    rebuildProcess.WaitForExit();
                }
            }
            catch(Exception e) {
                UnityEngine.Debug.LogError("There was a problem rebuilding assemblies.");
                UnityEngine.Debug.LogError(e);
            }
        }
#endif
        var compressed = false;
        var mapSources = MapDatabase.GetMaps();
        var mapOutputs = mapSources.Select(map => new MapBuildOutputs(compressed, map)).ToArray();
        PreBuildAssetBundles(mapSources);
        var OutputDirectory = BuildConstants.BuiltBundlesDirectory(compressed);
        CleanUpOutputDirectoryPreBuild(OutputDirectory);
        BuildAssetBundles(compressed);
        WriteMapProperties(mapOutputs);
        WriteMapZips(mapOutputs, compressed);
        CleanUpOutputDirectoryPostBuild(OutputDirectory);
        CopyToTestMapsDirectory(mapOutputs, BuildConstants.PluginName);
        UnityEngine.Debug.Log("Done building assets!");
    }

    private static void CopyToTestMapsDirectory(MapBuildOutputs[] mapOutputs, string pluginName) {
        var outDirPath = Preferences.instance.general.testMapDirectory;
        if(outDirPath == null || outDirPath == "") {
            PreferencesWindow.ShowWindow();
            EditorUtility.DisplayDialog(
                "Test Maps Directory not set",
                "Please set Test Map Directory in Preferences. Maps will be copied here when built.",
                "Ok", null);
                throw new Exception("Build aborted");
        }

        Directory.CreateDirectory(outDirPath);

        foreach(var map in mapOutputs) {
            var outZipPath = Path.Join(outDirPath, map.Sources.Name + PathConstants.MapFileExtension);
            File.Copy(map.BuiltZipPath, outZipPath, true);
        }
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

    private static void PreBuildAssetBundles(EditorMapDatabaseEntry[] maps) {
        // Simulate Ctrl+S to save open scene or open prefab
        EditorApplication.ExecuteMenuItem("File/Save");
        EditorSceneManager.SaveOpenScenes();

        AssetDatabase.RemoveUnusedAssetBundleNames();
        foreach(var map in maps) {
            AddAssetsToBundles(map);
        }
        // Save the changes
        AssetDatabase.SaveAssets();
    }

    /// <summary>
    /// Put all scenes and assets in a map's subdirectory into the map's assetbundles
    /// </summary>
    private static void AddAssetsToBundles(EditorMapDatabaseEntry map)
    {
        // Get all assets in a folder and its subfolders, excluding scenes and scripts
        string assetDirectory = map.AssetDirectory;
        string[] assetGuids = AssetDatabase.FindAssets("", new[] { assetDirectory });

        foreach (string assetPath in assetGuids)
        {
            string assetPathWithExtension = AssetDatabase.GUIDToAssetPath(assetPath);

            // Scene goes into the scenes bundle, everything else into the assets bundle
            if (assetPathWithExtension.EndsWith(".unity"))
            {
                AssetImporter.GetAtPath(assetPathWithExtension).SetAssetBundleNameAndVariant(map.SceneBundleName, AssetNames.BundleVariant);
            }
            else if (!assetPathWithExtension.EndsWith(".cs"))
            {
                // Add the asset to the specified asset bundle
                AssetImporter.GetAtPath(assetPathWithExtension).SetAssetBundleNameAndVariant(map.AssetsBundleName, AssetNames.BundleVariant);
            }
        }
    }

    private static void CleanUpOutputDirectoryPreBuild(string directory) {
        // Do not delete emitted bundles, so that unity skips rebuild when
        // they haven't changed

        // if (Directory.Exists(directory))
        //     Directory.Delete(directory, true);
    }

    private static void CleanUpOutputDirectoryPostBuild(string directory) {

    }

    private static void BuildAssetBundles(bool compressed) {
        string directory = BuildConstants.BuiltBundlesDirectory(compressed);

        var options = BuildAssetBundleOptions.StrictMode;
        if(compressed) {
            options |= BuildAssetBundleOptions.UncompressedAssetBundle;
        }

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        

        // TODO this winterland code is smart and only builds necessary bundles.
        // I'm experimenting w/being lazy, letting Unity do all the work.
        // Unity only rebuilds assetbundles when necessary, which might be easier.

        // var assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
        // assetBundleNames = assetBundleNames.Where((assetBundleName) => {
        //     if (assetBundleName == "winter") return true;
        //     if (assetBundleName.StartsWith("stages/")) return true;
        //     return false;
        // }).ToArray();

        // var builds = new List<AssetBundleBuild>();

        // foreach(var assetBundle in assetBundleNames) {
        //     var assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundle);

        //     var build = new AssetBundleBuild();
        //     build.assetBundleName = "winter.asset.bundle";
        //     if (assetBundle.StartsWith("stages/"))
        //         build.assetBundleName = $"{assetBundle.Substring(7)}.stage.bundle";
        //     build.assetNames = assetPaths;
        //     builds.Add(build);
        // }
        // var manifest = BuildPipeline.BuildAssetBundles(directory, builds.ToArray(), options, BuildTarget.StandaloneWindows64);

        MapBuilderStatus.IsBuilding = true;
        AssetBundleManifest manifest;
        try {
            manifest = BuildPipeline.BuildAssetBundles(directory, options, BuildTarget.StandaloneWindows64);
        } finally {
            MapBuilderStatus.IsBuilding = false;
        }

        if(manifest == null) {
            throw new Exception("Building asset bundles failed!");
        }
    }

    private static void WriteMapProperties(MapBuildOutputs[] maps) {
        foreach(var map in maps) {
            var propertiesAsset = AssetDatabase.LoadAssetAtPath<MapPropertiesScriptableObject>(map.Sources.PropertiesPath);
            var outputPath = map.BuiltPropertiesPath;
            File.WriteAllText(outputPath, JsonUtility.ToJson(propertiesAsset.properties));
        }
    }

    private static void WriteMapZips(MapBuildOutputs[] maps, bool compressed) {
        foreach(var map in maps) {
            if(File.Exists(map.BuiltZipPath)) {
                File.Delete(map.BuiltZipPath);
            }
            new MapZip(map.BuiltZipPath).WriteZip(
                propertiesContents: File.ReadAllText(map.BuiltPropertiesPath), 
                sceneBundlePath: map.BuiltSceneBundlePath,
                assetsBundlePath: map.BuiltAssetsBundlePath,
                compressed
            );
        }
    }
}

/// <summary>
/// Paths to intermediate and final build artifacts for a map
/// </summary>
class MapBuildOutputs {
    private bool compressed;
    private EditorMapDatabaseEntry sources;
    public EditorMapDatabaseEntry Sources => sources;

    public MapBuildOutputs(bool compressed, EditorMapDatabaseEntry map) {
        this.compressed = compressed;
        this.sources = map;
    }

    public string BuiltDirectory => BuildConstants.BuiltBundlesDirectory(compressed) + "/" + AssetNames.BundlePrefix + sources.Name;
    public string BuiltPropertiesPath => Path.Join(BuiltDirectory, MapZip.propertiesFilename);
    public string BuiltAssetsBundlePath => Path.Join(BuiltDirectory, AssetNames.AssetsBundleBasename);
    public string BuiltSceneBundlePath => Path.Join(BuiltDirectory, AssetNames.SceneBundleBasename);
    public string BuiltZipPath => Path.Join(BuiltDirectory, BuildConstants.BuiltZipFilename);
}
