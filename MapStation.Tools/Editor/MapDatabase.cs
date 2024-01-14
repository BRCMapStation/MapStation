using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using MapStation.Common;

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
            };
        }

    }

    public class EditorMapDatabaseEntry : Common.BaseMapDatabaseEntry {
        public string PropertiesPath;
        public string AssetDirectory => AssetNames.GetAssetDirectoryForMap(Name);

        // Unused: idea I had to find all scene assets, then log a helpful message telling the user it's confusing to have multiple scenes
        // public string[] extantSceneCandidatePaths;
    }
}