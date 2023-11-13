using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using Reptile;

namespace Winterland.Plugin {
    public class WinterAssets {
        private Dictionary<string, AssetBundle> bundleByStageName = null;
        public WinterAssets(string folder) {
            bundleByStageName = new();

            var stagesFolder = Path.Combine(folder, "stages");
            if (Directory.Exists(stagesFolder)) {
                var bundles = Directory.GetFiles(stagesFolder, "*", SearchOption.TopDirectoryOnly);
                foreach(var file in bundles) {
                    var bundle = AssetBundle.LoadFromFile(file);
                    bundleByStageName[Path.GetFileName(file)] = bundle;
                }
            }
        }

        public GameObject GetPrefabForStage(Stage stage) {
            var stageName = stage.ToString().ToLowerInvariant();
            if (!bundleByStageName.TryGetValue(stageName, out var bundle))
                return null;
            return bundle.LoadAsset<GameObject>(stageName);
        }
    }
}
