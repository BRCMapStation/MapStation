using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using MapStation.Common;
using MapStation.Common.Doctor;
using MapStation.Tools;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MapBuilder {
#if MAPSTATION_DEBUG
    [MenuItem(UIConstants.menuLabel + "/Build Maps and Run on Steam _F6", priority = (int)UIConstants.MenuOrder.BUILD_ASSETS_AND_RUN_ON_STEAM)]
    private static void BuildAndRunSteam() {
        BuildAssets();
        GameLauncher.LaunchGameSteam();
    }
#endif

    [MenuItem(UIConstants.menuLabel + "/Build Maps and Run on Steam _F6", true)]
    private static bool BuildAndRunSteamValidate() {
        return (!GameLauncher.IsGameOpen() && GameLauncher.CanLaunchOnSteam());
    }

    [MenuItem(UIConstants.menuLabel + "/Build Maps _F5", priority = (int)UIConstants.MenuOrder.BUILD_ASSETS)]
    public static void BuildAssets() {
        var mapOutputs = BuildAllAssetBundles(compressed: false);
        CopyToTestMapsDirectory(mapOutputs, BuildConstants.PluginName);
        UnityEngine.Debug.Log("Done building assets!");
    }

    [MenuItem(UIConstants.menuLabel + "/Package for Thunderstore", priority = (int)UIConstants.MenuOrder.BUILD_AND_PACKAGE_FOR_THUNDERSTORE)]
    private static void BuildAndPackageForThunderstore() {
        var maps = BuildAllAssetBundles(compressed: true);
        BuildThunderstoreZips(maps);
        ShowExplorer(Path.Combine(Path.GetDirectoryName(Application.dataPath), BuildConstants.BuiltThunderstoreZipsDirectory));
        UnityEngine.Debug.Log("Done building Thunderstore zips!");
    }

    private static MapBuildOutputs[] BuildAllAssetBundles(bool compressed = false) {
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
        var mapSources = MapDatabase.GetMaps();
        var mapOutputs = mapSources.Select(map => new MapBuildOutputs(compressed, map)).ToArray();
        ValidateAndFixSceneNames(mapSources);
        StubMissingMapFiles(mapSources);
        SyncMapProperties(mapSources);
        PreBuildAssetBundles(mapSources);
        CheckForDoctorErrors();
        var OutputDirectory = BuildConstants.BuiltBundlesDirectory(compressed);
        CleanUpOutputDirectoryPreBuild(OutputDirectory);
        BuildAssetBundles(compressed);
        WriteMapProperties(mapOutputs);
        WriteMapZips(mapOutputs, compressed);
        CleanUpOutputDirectoryPostBuild(OutputDirectory);
        return mapOutputs;
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

    private static void ValidateAndFixSceneNames(EditorMapDatabaseEntry[] maps) {
        foreach(var map in maps) {
            var sceneAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(map.ScenePath);
            if(sceneAsset == null) {
                var message = "Missing scene";
                EditorUtility.DisplayDialog(message,
                    $"Scene is missing for {map.Name}.\n" + 
                    "\n" +
                    "Double-check that your scene's filename is correct. It must exactly match the required path.\n" +
                    "\n" +
                    "Expected:\n" +
                    map.ScenePath[0..^6],
                    "Ok", null);
                    throw new Exception(message);
            }
            var sceneAssetName = AssetDatabase.GetAssetPath(sceneAsset);
            if (sceneAssetName != map.ScenePath) {
                // Must be different capitalization; unity's AssetDatabase is case insensitive, but runtime scene loading is case sensitive.
                // Automatically fix capitalization to avoid confusion.
                AssetDatabase.RenameAsset(map.ScenePath, map.ScenePath);
                AssetDatabase.SaveAssets();
            }
        }
    }

    public static void StubMissingMapFiles(EditorMapDatabaseEntry[] maps) {
        foreach(var map in maps) {
            if(!File.Exists(map.ReadmePath)) {
                File.Create(map.ReadmePath);
            }
            if(!File.Exists(map.ChangelogPath)) {
                File.Create(map.ChangelogPath);
            }
            if(!File.Exists(map.IconPath)) {
                File.WriteAllBytes(map.IconPath, File.ReadAllBytes(ToolAssetConstants.DefaultThunderstoreIconPath));
            }
            if(map.Properties == null) {
                MapDatabase.CreatePropertiesForMap(map);
            }
        }
    }

    public static void SyncMapProperties(EditorMapDatabaseEntry[] maps) {
        foreach(var map in maps) {
            map.Properties.SyncAutomaticFields(map);
        }
        // Save the changes
        AssetDatabase.SaveAssets();
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
                // NOT DOING THIS
                // Turns out, assigning a mesh to a different assetbundle than the scene means we lose baked collision information.
                // Leaving the un-assigned ensures it's bundled into the scene's bundle as a dependency.
                
                // Only other workaround would be using the mesh as a MeshCollider in a prefab in the assets bundle, which triggers
                // inclusion of baked collision data alongside the mesh.

                // Add the asset to the specified asset bundle
                // AssetImporter.GetAtPath(assetPathWithExtension).SetAssetBundleNameAndVariant(map.AssetsBundleName, AssetNames.BundleVariant);
                AssetImporter.GetAtPath(assetPathWithExtension).SetAssetBundleNameAndVariant(null, null);
                // AssetImporter.GetAtPath(assetPathWithExtension).SetAssetBundleNameAndVariant(map.AssetsBundleName, AssetNames.BundleVariant);
            }

            // Lazy: Ensure the bundle is generated by putting at least one thing into it: the Properties ScriptableObject
            // In future, we can ensure this w/ the MiniMap prefab.
            // Is confusing because, though Properties ScriptableObject *can* go into a bundle, we don't care; we read properties.json instead.
            AssetImporter.GetAtPath(map.PropertiesPath).SetAssetBundleNameAndVariant(map.AssetsBundleName, AssetNames.BundleVariant);
        }
    }

    private static void CheckForDoctorErrors() {
        if (!Preferences.instance.general.checkDoctorErrorsBeforeBuildingMap) return;
        var analysis = Doctor.Analyze();
        var errors = analysis.countBySeverity[Severity.Error];
        if (errors > 0) {
            var message = $"Map Doctor found {errors} errors that may crash the game. Are you sure you want to build?";
            var response = EditorUtility.DisplayDialog(
                "Fatal Map Errors",
                message,
                "Build Map", "See Errors");
            if (!response) {
                DoctorWindow.ShowWindow().Analysis = analysis;
                throw new Exception("Build aborted");
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
            var propertiesAsset = map.Sources.Properties;
            var outputPath = map.BuiltPropertiesPath;
            File.WriteAllText(outputPath, JsonUtility.ToJson(propertiesAsset.properties, true));
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

    private static void BuildThunderstoreZips(MapBuildOutputs[] maps) {
        Directory.CreateDirectory(BuildConstants.BuiltThunderstoreZipsDirectory);
        foreach(var map in maps) {
            BuildThunderstoreZip(map);
        }
    }

    private static void BuildThunderstoreZip(MapBuildOutputs map) {
        var zipPath = map.BuiltThunderstoreZipPath;
        if(File.Exists(zipPath)) {
            File.Delete(zipPath);
        }
        ZipArchive zip = ZipFile.Open(zipPath, ZipArchiveMode.Create);

        // manifest.json
        var manifest = new ThunderstoreManifest() {
            name = map.Sources.Properties.thunderstoreName,
            author = map.Sources.Properties.properties.authorName,
            version_number = map.Sources.Properties.properties.version,
            description = map.Sources.Properties.description,
            website_url = map.Sources.Properties.website,
            dependencies = PluginManager.GetDependencies()
        };
        var manifestContents = JsonUtility.ToJson(manifest, true);
        var manifestEntry = zip.CreateEntry("manifest.json");
        using(var file = manifestEntry.Open())
        using(var writer = new StreamWriter(file)) {
            writer.Write(manifestContents);
        }

        // .brcmap
        zip.CreateEntryFromFile(map.BuiltZipPath, map.Sources.Name + PathConstants.MapFileExtension);

        // Other
        zip.CreateEntryFromFile(map.Sources.ReadmePath, "README.md");
        zip.CreateEntryFromFile(map.Sources.ChangelogPath, "CHANGELOG.md");
        zip.CreateEntryFromFile(map.Sources.IconPath, "icon.png");

        PluginManager.ProcessThunderstoreZip(zip, map.Sources.Name);

        // Write it!
        zip.Dispose();
    }

    private static void ShowExplorer(string path) {
        // explorer doesn't like forward slashes
        path = path.Replace(@"/", @"\");
        Debug.Log(path);
        System.Diagnostics.Process.Start("explorer.exe", path);
        // System.Diagnostics.Process.Start("explorer.exe", "/select,"+path);
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
    public string BuiltThunderstoreZipPath => Path.Join(BuildConstants.BuiltThunderstoreZipsDirectory, $"{sources.Name}-{sources.Properties.properties.version}.zip");
}

[Serializable]
class ThunderstoreManifest {
    public string name;
    public string author;
    public string version_number;
    public string website_url;
    public string description;
    public List<string> dependencies;
}
