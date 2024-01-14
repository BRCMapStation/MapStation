namespace MapStation.Common {
    
    /// <summary>
    /// Map "Database" is a glorified list of all maps.
    /// 
    /// This class is shared across Plugin and Editor, subclassed to add plugin- and editor-specific stuff.
    /// </summary>
    public class BaseMapDatabaseEntry {
        public string Name;
        public string ScenePath;
        public string SceneBundleName => AssetNames.GetSceneBundleNameForMap(Name);
        public string AssetsBundleName => AssetNames.GetAssetBundleNameForMap(Name);

        // Unused: idea I had to find all scene assets, then log a helpful message telling the user it's confusing to have multiple scenes
        // public string[] extantSceneCandidatePaths;
    }
}