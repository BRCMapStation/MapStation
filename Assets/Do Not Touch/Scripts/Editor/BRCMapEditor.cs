using UnityEngine;
using UnityEditor;
using System.IO;
using System;

[CustomEditor(typeof(BRCMap))]
public class BRCMapEditor : Editor
{
    private BRCMap thisMap;

    private void Awake()
    {
        thisMap = target as BRCMap;
        Debug.Log(thisMap);
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.HelpBox("You should have your own folder in the Assets folder with the exact same name as your map name entered in the field above." +
            Environment.NewLine +
            Environment.NewLine +
            "All the assets used by your map should be contained in this folder and subfolders. " +
            "Certain file types, such as scenes and scripts, will be ignored as they cannot be used." +
            Environment.NewLine +
            Environment.NewLine +
            "If building your map is successful, it will appear in the Map Files folder.", MessageType.Info);

        if (GUILayout.Button("Toggle Red Debug Shapes"))
        {
            // Apply any changes made to the prefab
            ToggleDebugObjects();
        }

        if (GUILayout.Button("Create MapPrefab from Scene"))
        {
            // Apply any changes made to the prefab
            CreateMapPrefabFromScene();
        }

        if (GUILayout.Button("Collect Map Assets"))
        {
            // Apply any changes made to the prefab
            AddAssetsToBundle();
        }

        if (GUILayout.Button("Build Map File"))
        {
            // Apply any changes made to the prefab
            CreateMapBundle();
        }
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
        }

        // Save the changes
        AssetDatabase.SaveAssets();
    }

    void CreateMapBundle()
    {
        string assetBundleDirectory = "Assets/Map Files";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        try
        {
            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.StrictMode, BuildTarget.StandaloneWindows);
            DirectoryInfo d = new DirectoryInfo(assetBundleDirectory);
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
            Debug.Log("<b>✔️ SUCCESSFULLY BUILT BRC MAP FILE. WOOHOO!!!! ✔️</b>");
        }
        catch (Exception e)
        {
            Debug.LogError("Something's wrong with your map. The map file was not generated." + e.ToString());
        }
    }

    private void CreateMapPrefabFromScene()
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
                GameObject objClone = Instantiate(obj, mapPrefab.transform);
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
        DestroyImmediate(mapPrefab);

        Debug.Log("MapPrefab created from the current scene and saved as a prefab.");
    }
    
    private void ToggleDebugObjects()
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