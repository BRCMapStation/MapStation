namespace MapStation.Tools {
    public static class BuildConstants {
        public static string BuiltBundlesDirectory(bool compressed) {
            return compressed ? BuiltCompressedDirectory : BuiltUncompressedDirectory;
        }
        public const string BuiltUncompressedDirectory = "Assets/Temp/Bundles";
        public const string BuiltCompressedDirectory = "Assets/Temp/CompressedBundles";
        public const string BuiltZipFilename = "map" + Common.PathConstants.MapFileExtension;
        public const string PluginName = "MapStation";
    }
}