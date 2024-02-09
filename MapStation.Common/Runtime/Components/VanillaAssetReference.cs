using System;
using System.Collections.Generic;
using System.Reflection;
using Reptile;
using UnityEngine;

namespace MapStation.Common.VanillaAssets {
    /// <summary>
    /// We can't reference vanilla game assets from our asset bundles. The cross-bundle references
    /// are broken.
    /// 
    /// This component uses reflection to assign fields at startup, referencing vanilla assets.
    /// </summary>
    public class VanillaAssetReference : MonoBehaviour {
        public Component component = null;
        // Structs don't deserialize from BepInEx plugins
        [TextArea(3,10)]
        public List<string> fields = new ();

        private void Awake() {
#if BEPINEX
            AssignReferences();
#endif
        }

        public const BindingFlags UseTheseBindingFlags =
            BindingFlags.Instance
            | BindingFlags.Public
            | BindingFlags.NonPublic
            | BindingFlags.FlattenHierarchy;

#if BEPINEX
        public void AssignReferences() {
            if(component == null) return;
            foreach(var fieldSyntax in fields) {
                // Parse string into field, bundle, and asset path
                var equalsIndex = fieldSyntax.IndexOf("=");
                var colonIndex = fieldSyntax.IndexOf(":");
                var propertyPath = fieldSyntax.Substring(0, equalsIndex);
                var index = -1;
                var indexOfBracket = propertyPath.IndexOf("[");
                if(indexOfBracket >=0) {
                    index = int.Parse(propertyPath.Substring(indexOfBracket + 1, -1));
                    name = propertyPath.Substring(0, indexOfBracket);
                } else {
                    name = propertyPath;
                }
                var bundle = fieldSyntax.Substring(equalsIndex + 1, colonIndex - equalsIndex - 1);
                var path = fieldSyntax.Substring(colonIndex + 1);

                // Get asset
                var asset = Core.Instance.Assets.LoadAssetFromBundle<UnityEngine.Object>(bundle, path);

                if(asset == null) {
                    Log.Info(string.Format("{0}: Restoring reference to vanilla asset failed, asset not found: {1}.{2} = LoadAssetFromBundle(\"{3}\", \"{4}\")", nameof(VanillaAssetReference), component.GetType().Name, name, bundle, path));
                    continue;
                }

                // Check for both private and public fields
                var componentType = component.GetType();
                var member = componentType.GetMember(name, UseTheseBindingFlags)[0];

                var message = string.Format(nameof(VanillaAssetReference) + ": Assigning {0}.{1} = asset {2}:{3} (asset found={4}, field found={5}, asset type={6})", componentType.Name, propertyPath, bundle, path, asset != null, member != null, asset != null ? asset.GetType().Name : "<not found>");

                try {
                    if(index >= 0) {
                        var collection = member is PropertyInfo p ? p.GetValue(component) : ((FieldInfo)member).GetValue(component);
                        collection.GetType().GetProperty("Item").SetValue(collection, asset, new object[]{index});
                    } else {
                        if(member is PropertyInfo p) {
                            p.SetValue(component, asset);
                        } else {
                            ((FieldInfo)member).SetValue(component, asset);
                        }
                    }
                } catch(Exception e) {
                    Log.Info(message + "\nFailed with error:\n" + e.Message + "\n" + e.StackTrace);
                }
            }
        }
#endif
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
