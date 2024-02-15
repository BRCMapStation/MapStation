namespace MapStation.Tools {
    public static class BuildConstants {
        public static string BuiltBundlesDirectory(bool compressed) {
            return compressed ? BuiltCompressedDirectory : BuiltUncompressedDirectory;
        }
        public const string BuiltUncompressedDirectory = "Assets/Temp/Uncompressed";
        public const string BuiltCompressedDirectory = "Assets/Temp/Compressed";
        public const string BuiltZipFilename = "map" + Common.PathConstants.MapFileExtension;
        public const string PluginName = "MapStation";
        public const string BuiltThunderstoreZipsDirectory = "Assets/Thunderstore";
        /// Maps packaged for thunderstore declare a dependency on the mapstation plugin
        public const string ThunderstoreMapstationDependency = "SlopBrew-MapStation-1.0.0";
    }

    /// <summary>
    /// Assets included in our tools
    /// </summary>
    public static class ToolAssetConstants {
        public const string DefaultThunderstoreIconPath = "Packages/com.brcmapstation.tools/Assets/Icons/DefaultThunderstoreIcon.png";
        public const string NewMapTemplatePath = "Packages/com.brcmapstation.tools/Assets/NewMapTemplate";
        public const string NewMapTemplateScenePath = "Packages/com.brcmapstation.tools/Assets/NewMapTemplate/Scene.unity";
    }
}
