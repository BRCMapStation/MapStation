using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using Reptile;

namespace Winterland.Common {
    /// <summary>
    /// Retrieve stuff from our asset bundles using this class.
    /// </summary>
    public class WinterAssets {
        private readonly Dictionary<string, AssetBundle> bundleByStageName = null;
        public AssetBundle WinterBundle = null;
        public static WinterAssets Instance { get; private set; }

        public WinterAssets(string folder) {
            Instance = this;
            bundleByStageName = new();
            var winterBundleLocation = Path.Combine(folder, "winter");
            if (File.Exists(winterBundleLocation))
                WinterBundle = AssetBundle.LoadFromFile(winterBundleLocation);
            var stagesFolder = Path.Combine(folder, "stages");
            if (Directory.Exists(stagesFolder)) {
                var bundles = Directory.GetFiles(stagesFolder, "*", SearchOption.TopDirectoryOnly);
                foreach(var file in bundles) {
                    var bundle = AssetBundle.LoadFromFile(file);
                    bundleByStageName[Path.GetFileName(file)] = bundle;
                }
            }
        }

        /// <summary>
        /// Returns the prefab containing the additions for a stage.
        /// </summary>
        public GameObject GetPrefabForStage(Stage stage) {
            var stageName = stage.ToString().ToLowerInvariant();
            if (!bundleByStageName.TryGetValue(stageName, out var bundle))
                return null;
            return bundle.LoadAsset<GameObject>(stageName);
        }
    }
}
