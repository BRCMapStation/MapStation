using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;

public class BuildAssets {

    private const string OutputDirectory = "Build";

    [MenuItem("BRC/Build Assets")]
    private static void BuildAllAssetBundles() {
        PreBuildAssetBundles();
        CleanUpOutputDirectoryPreBuild(OutputDirectory);
        BuildAssetBundles(OutputDirectory);
        CleanUpOutputDirectoryPostBuild(OutputDirectory);
        if (Directory.Exists(OutputDirectory))
            Process.Start(OutputDirectory);
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
