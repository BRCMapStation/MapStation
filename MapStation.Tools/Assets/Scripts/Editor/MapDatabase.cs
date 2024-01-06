using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MapStation.Tools {
    /// <summary>
    /// Queries Unity's AssetDatabase to find all maps in your project, based on naming conventions.
    /// 
    /// Also declares how we organize maps into assetbundles.
    /// </summary>
    public static class MapDatabase {
        public const string MapDirectory = "Maps";
        public const string SceneBasename = "Scene";
        public const string PropertiesBasename = "Properties";
        public const string BundlePrefix = "maps/";
        public const string SceneBundleBasename = "scene";
        public const string SceneBundleSuffix = "/" + SceneBundleBasename;
        public const string AssetsBundleBasename = "assets";
        public const string AssetsBundleSuffix = "/" + AssetsBundleBasename;
        public const string BundleVariant = "";

        [InitializeOnLoadMethod]
        public static MapDatabaseEntry[] GetMaps() {
            List<MapDatabaseEntry> entries = new ();
            foreach(var folder in AssetDatabase.GetSubFolders($"Assets/{MapDirectory}")) {
                var name = folder.Substring(folder.LastIndexOf("/") + 1);
                entries.Add(GetMap(name));
            }
            return entries.ToArray();
        }

        public static MapDatabaseEntry GetMap(string name) {
            return new MapDatabaseEntry {
                Name = name,
                ScenePath = GetScenePathForMap(name),
                PropertiesPath = GetPropertiesPathForMap(name),
            };
        }

        public static string GetAssetDirectoryForMap(string name) {
            return $"Assets/{MapDirectory}/{name}";
        }

        public static string GetScenePathForMap(string name) {
            return $"Assets/{MapDirectory}/{name}/{SceneBasename}.unity";
        }

        public static string GetPropertiesPathForMap(string name) {
            return $"Assets/{MapDirectory}/{name}/{PropertiesBasename}.asset";
        }
    }

    public class MapDatabaseEntry {
        public string Name;
        public string ScenePath;
        public string PropertiesPath;
        public string AssetDirectory => MapDatabase.GetAssetDirectoryForMap(Name);
        public string SceneBundleName => $"{MapDatabase.BundlePrefix}{Name.ToLower()}{MapDatabase.SceneBundleSuffix}";
        public string AssetsBundleName => $"{MapDatabase.BundlePrefix}{Name.ToLower()}{MapDatabase.AssetsBundleSuffix}";

        // Unused: idea I had to find all scene assets, then log a helpful message telling the user it's confusing to have multiple scenes
        // public string[] extantSceneCandidatePaths;
    }
}