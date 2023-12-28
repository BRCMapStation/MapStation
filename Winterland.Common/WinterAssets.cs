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

        private string folder;
        public AudioClip CheckpointClip = null;
        public AnimationClip PlayerSantaBounce = null;

        public WinterAssets(string folder) {
            Instance = this;
            bundleByStageName = new();
            this.folder = folder;
            LoadBundles();
            CheckpointClip = WinterBundle.LoadAsset<AudioClip>("checkpoint");
            PlayerSantaBounce = WinterBundle.LoadAsset<AnimationClip>("playerSantaBounce");
        }
        
        private void LoadBundles() {
            var winterBundleLocation = Path.Combine(folder, "winter.asset.bundle");

            if (File.Exists(winterBundleLocation))
                WinterBundle = AssetBundle.LoadFromFile(winterBundleLocation);
            var stageBundles = Directory.GetFiles(folder, "*.stage.bundle");

            foreach(var stageBundle in stageBundles) {
                // this kinda ugly actually!
                var stageName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(stageBundle));
                Debug.Log($"Found bundle {stageBundle} for stage {stageName}");
                bundleByStageName[stageName] = AssetBundle.LoadFromFile(stageBundle);
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

        /// <summary>
        /// Reload asset bundles, for development, rapid iteration
        /// </summary>
        public void ReloadBundles() {
            UnloadBundles();
            LoadBundles();
        }

        private void UnloadBundles() {
            WinterBundle?.Unload(false);
            WinterBundle = null;
            foreach(var bundle in bundleByStageName) {
                bundle.Value.Unload(false);
            }
            bundleByStageName.Clear();
        }
    }
}
