using System;
using System.Collections.Generic;
using System.Reflection;
using Reptile;
using UnityEngine;

namespace Winterland.MapStation.Common.VanillaAssets {
    /// <summary>
    /// We can't reference vanilla game assets from our asset bundles. The cross-bundle references
    /// are broken.
    /// 
    /// This component uses reflection to assign fields at startup, referencing vanilla assets.
    /// </summary>
    public class VanillaAssetReference : MonoBehaviour {
        public MonoBehaviour component = null;
        // Structs don't deserialize from BepInEx plugins
        [TextArea(3,10)]
        public List<string> fields = new ();

        private void Awake() {
            AssignReferences();
        }

        public void AssignReferences() {
            if(component == null) return;
            foreach(var f in fields) {
                // Parse string into field, bundle, and asset path
                var equalsIndex = f.IndexOf("=");
                var colonIndex = f.IndexOf(":");
                var name = f.Substring(0, equalsIndex);
                var bundle = f.Substring(equalsIndex + 1, colonIndex - equalsIndex - 1);
                var path = f.Substring(colonIndex + 1);

                // Get asset
                var asset = Core.Instance.Assets.LoadAssetFromBundle<UnityEngine.Object>(bundle, path);

                if(asset == null) {
                    Debug.Log(string.Format("{0}: Restoring reference to vanilla asset failed, asset not found: {1}.{2} = LoadAssetFromBundle(\"{3}\", \"{4}\")", nameof(VanillaAssetReference), component.GetType().Name, name, bundle, path));
                    continue;
                }

                // Check for both private and public fields
                var componentType = component.GetType();
                var field = componentType.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                var message = string.Format(nameof(VanillaAssetReference) + ": Assigning {0}.{1} = asset {2}:{3} (asset found={4}, field found={5}, asset type={6})", componentType.Name, name, bundle, path, asset != null, field != null, asset != null ? asset.GetType().Name : "<not found>");

                try {
                    field.SetValue(component, asset);
                } catch(Exception e) {
                    Debug.Log(message + "\nFailed with error:\n" + e.Message + "\n" + e.StackTrace);
                }
            }
        }
    }

    // I wanted to use these structs to store state, but deserialization for
    // BepInEx plugin DLLs is broken.
    // See https://github.com/xiaoxiao921/FixPluginTypesSerialization

    // [Serializable]
    // public struct ComponentEntry {
    //     [SerializeField]
    //     public Component component;
    //     [SerializeField]
    //     public List<FieldEntry> fields = [];

    //     public ComponentEntry() {
    //     }
    // }

    // [Serializable]
    // public struct FieldEntry {
    //     [SerializeField]
    //     public string Name;
    //     [SerializeField]
    //     public string BundleName;
    //     [SerializeField]
    //     public string Path;
    //     public FieldEntry() {
    //     }
    // }
}
