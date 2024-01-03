using System;
using System.IO;
using UnityEditor;
using Object = UnityEngine.Object;
using UnityEngine;

/// <summary>
/// Logic to build BRC maps, separated from the UI / Inspectors / windows.
/// </summary>
public class BRCMapBuilder {

    BRCMap thisMap;
    public BRCMapBuilder(BRCMap map) {
        thisMap = map;
    }

    public void RebuildAll() {
        CreateMapPrefabFromScene();
        AddAssetsToBundle();
        CreateMapBundle();
    }

    public void AddAssetsToBundle()
    {
        // Get all assets in a folder and its subfolders, excluding scenes and scripts
        string folderPath = "Assets/" + thisMap.mapName; // Use the path based on the map name
        string[] assetPaths = AssetDatabase.FindAssets("", new[] { folderPath });

        foreach (string assetPath in assetPaths)
        {
            string assetPathWithExtension = AssetDatabase.GUIDToAssetPath(assetPath);

            // Check if the asset is not a scene or script, those don't work like other assets I guess :(
            if (!assetPathWithExtension.EndsWith(".unity") && !assetPathWithExtension.EndsWith(".cs"))
            {
                // Add the asset to the specified asset bundle
                AssetImporter.GetAtPath(assetPathWithExtension).SetAssetBundleNameAndVariant(thisMap.mapName, "sup");

                Debug.Log("Adding " + assetPathWithExtension + " to the collection of map assets...");
            }
            // if (assetPathWithExtension.EndsWith(".unity")) {
            //     AssetImporter.GetAtPath(assetPathWithExtension).SetAssetBundleNameAndVariant(thisMap.mapName + "_scenes", "sup");
            // }
        }

        // Save the changes
        AssetDatabase.SaveAssets();
    }

    public void CreateMapBundle()
    {
        string assetBundleDirectory = "Assets/Map Files";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        try
        {
            DirectoryInfo d = new DirectoryInfo(assetBundleDirectory);
            foreach (var file in d.GetFiles("*.sup"))
            {
                file.Delete();
            }
            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.StrictMode, BuildTarget.StandaloneWindows);
            File.Delete("Assets/Map Files/Map Files");
            foreach (var file in d.GetFiles("*.manifest"))
            {
                file.Delete();
            }
            foreach (var file in d.GetFiles("*.manifest.meta"))
            {
                file.Delete();
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

    public void CreateMapPrefabFromScene()
    {
        // Find all objects in the current scene, excluding this script's GameObject.
        GameObject[] sceneObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        // Create a new empty GameObject to hold the scene objects.
        GameObject mapPrefab = new GameObject("MapPrefab");

        foreach (GameObject obj in sceneObjects)
        {
            if (obj != thisMap.gameObject) // Exclude this script's GameObject.
            {
                // Duplicate the object and make it a child of the mapPrefab.
                GameObject objClone = Object.Instantiate(obj, mapPrefab.transform);
                objClone.name = obj.name; // Ensure the clone has the same name.

                // Set the transform properties to match the original object.
                objClone.transform.position = obj.transform.position;
                objClone.transform.rotation = obj.transform.rotation;
                objClone.transform.localScale = obj.transform.localScale;
            }
        }

        // Save the mapPrefab as a prefab asset to the map folder.
        string prefabPath = "Assets/" + thisMap.mapName + "/MapPrefab.prefab";
        PrefabUtility.SaveAsPrefabAsset(mapPrefab, prefabPath);

        // Destroy the mapPrefab in the scene.
        Object.DestroyImmediate(mapPrefab);

        Debug.Log("MapPrefab created from the current scene and saved as a prefab.");
    }
    
    public void ToggleDebugObjects()
    {
        Renderer[] renderers = GameObject.FindObjectsByType<Renderer>(FindObjectsSortMode.None);

        foreach(Renderer r in renderers)
        {
            if (r.sharedMaterial.name == "Debug")
            {
                r.enabled = !r.enabled;
            }
        }
    }
}