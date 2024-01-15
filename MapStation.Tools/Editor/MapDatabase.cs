using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using MapStation.Common;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace MapStation.Tools {
    /// <summary>
    /// Queries Unity's AssetDatabase to find all maps in your project, based on naming conventions.
    /// 
    /// Also declares how we organize maps into assetbundles.
    /// </summary>
    public static class MapDatabase {

        [InitializeOnLoadMethod]
        public static EditorMapDatabaseEntry[] GetMaps() {
            List<EditorMapDatabaseEntry> entries = new ();
            foreach(var folder in AssetDatabase.GetSubFolders($"Assets/{AssetNames.MapDirectory}")) {
                var name = folder.Substring(folder.LastIndexOf("/") + 1);
                entries.Add(GetMap(name));
            }
            return entries.ToArray();
        }

        public static EditorMapDatabaseEntry GetMap(string name) {
            return new EditorMapDatabaseEntry {
                Name = name,
                ScenePath = AssetNames.GetScenePathForMap(name),
                PropertiesPath = AssetNames.GetPropertiesPathForMap(name),
                ReadmePath = AssetNames.GetReadmePathForMap(name),
                ChangelogPath = AssetNames.GetChangelogPathForMap(name),
                IconPath = AssetNames.GetIconPathForMap(name),
            };
        }

        /// <summary>
        /// Get the map corresponding to the scene, even if the scene has the wrong filename.
        /// Only checks if the scene is within the map's directory.
        /// Checks active scene by default.
        /// </summary>
        public static EditorMapDatabaseEntry GetMapForActiveScene() {
            return GetMapForScene(EditorSceneManager.GetActiveScene());
        }

        public static EditorMapDatabaseEntry GetMapForScene(Scene scene) {
            return GetMapForAssetPath(scene.path);
        }

        public static EditorMapDatabaseEntry GetMapForAssetPath(string path) {
            var pathParts = path.Split('/');
            if(pathParts[0] == "Assets" && pathParts[1] == AssetNames.MapDirectory && pathParts.Length >= 4) {
                return GetMap(pathParts[2]);
            }
            return null;
        }

        public static void CreatePropertiesForMap(EditorMapDatabaseEntry map) {
            var properties = ScriptableObject.CreateInstance<MapPropertiesScriptableObject>();
            AssetDatabase.CreateAsset(properties, map.PropertiesPath);
        }
    }

    public class EditorMapDatabaseEntry : Common.BaseMapDatabaseEntry {
        public string PropertiesPath;
        public string ReadmePath;
        public string IconPath;
        public string ChangelogPath;
        public string AssetDirectory => AssetNames.GetAssetDirectoryForMap(Name);
        public MapPropertiesScriptableObject Properties => AssetDatabase.LoadAssetAtPath<MapPropertiesScriptableObject>(PropertiesPath);

        // Unused: idea I had to find all scene assets, then log a helpful message telling the user it's confusing to have multiple scenes
        // public string[] extantSceneCandidatePaths;
    }
}