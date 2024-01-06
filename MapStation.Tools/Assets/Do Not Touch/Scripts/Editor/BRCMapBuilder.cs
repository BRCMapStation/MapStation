using System;
using System.IO;
using UnityEditor;
using Object = UnityEngine.Object;
using UnityEngine;
using UnityEditor.Experimental;

/// <summary>
/// Logic to build BRC maps, separated from the UI / Inspectors / windows.
/// </summary>
public class BRCMapBuilder {
    private const string BuildDirectory = "Assets/Temp/Built";

    BRCMap thisMap;
    public BRCMapBuilder(BRCMap map) {
        thisMap = map;
    }

    public void RebuildAll() {
        CreateMapBundle();
    }


    public void CreateMapBundle()
    {
        string assetBundleDirectory = $"{BuildDirectory}";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        try
        {
            DirectoryInfo d = new DirectoryInfo(assetBundleDirectory);
            foreach (var file in d.GetFiles("*.sup")) // TODO stop using .sup extension
            {
                file.Delete();
            }
            var manifests = BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.StrictMode | BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneWindows);
            var allBundles = manifests.GetAllAssetBundles();
            File.Delete($"{BuildDirectory}/{BuildDirectory}");
            foreach (var file in d.GetFiles("*.manifest", new EnumerationOptions() {RecurseSubdirectories = true}))
            {
                // file.Delete();
            }
            foreach (var file in d.GetFiles("*.manifest.meta"))
            {
                // file.Delete();
            }
            AssetDatabase.Refresh();
            if(Preferences.instance.general.mapDirectory != "") {
                foreach (var file in d.GetFiles("*.sup")) {
                    file.CopyTo(Path.Join(Preferences.instance.general.mapDirectory, file.Name), true);
                }
            }
            Debug.Log("<b>✔️ SUCCESSFULLY BUILT BRC MAP FILE. WOOHOO!!!! ✔️</b>");
        }
        catch (Exception e)
        {
            Debug.LogError("Something's wrong with your map. The map file was not generated." + e.ToString());
        }
    }
}