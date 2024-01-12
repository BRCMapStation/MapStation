using System.IO;

namespace MapStation.Common {
    public class PathConstants {
        public const string MapFileExtension = ".brcmap";
        public const string ConfigDirectory = "MapStation";
        public const string TestMapsDirectory = "TestMaps";
        public static string AbsoluteTestMapsDirectoryFromBepInExProfile(string BepInExProfileDirectory) {
            return Path.Combine(BepInExProfileDirectory, "Config", ConfigDirectory, TestMapsDirectory);
        }
    }

    public class AssetNames {
        public const string MapDirectory = "Maps";
        public const string SceneBasenamePrefix = "Scene.";
        public const string PropertiesBasename = "Properties";
        public const string BundlePrefix = "maps/";
        public const string SceneBundleBasename = "scene";
        public const string SceneBundleSuffix = "/" + SceneBundleBasename;
        public const string AssetsBundleBasename = "assets";
        public const string AssetsBundleSuffix = "/" + AssetsBundleBasename;
        public const string BundleVariant = "";

        public static string GetAssetDirectoryForMap(string name) {
            return $"Assets/{MapDirectory}/{name}";
        }

        public static string GetScenePathForMap(string name) {
            return $"Assets/{MapDirectory}/{name}/{SceneBasenamePrefix}{name}.unity";
        }

        public static string GetPropertiesPathForMap(string name) {
            return $"Assets/{MapDirectory}/{name}/{PropertiesBasename}.asset";
        }

        public static string GetSceneBundleNameForMap(string name) {
            return $"{AssetNames.BundlePrefix}{name.ToLower()}{AssetNames.SceneBundleSuffix}";
        }
        
        public static string GetAssetBundleNameForMap(string name) {
            return $"{AssetNames.BundlePrefix}{name.ToLower()}{AssetNames.AssetsBundleSuffix}";
        }
    }
}
