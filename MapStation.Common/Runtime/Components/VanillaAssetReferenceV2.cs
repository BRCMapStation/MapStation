using System;
using System.Collections.Generic;
using System.Reflection;
using Reptile;
using UnityEngine;
using UnityEngine.Audio;
using MapStation.Common.Serialization;
using Object = UnityEngine.Object;

namespace MapStation.Common.VanillaAssets {
    /// <summary>
    /// We can't reference vanilla game assets from our asset bundles. The cross-bundle references
    /// are broken.
    /// 
    /// This component uses reflection to assign fields at startup, referencing vanilla assets.
    /// </summary>
    public class VanillaAssetReferenceV2 : MonoBehaviour {

        // BepInEx serialization workaround
        private List<ComponentEntry> Components => components.items;
        [SerializeReference] private SList_Components components = new ();
        public class SList_Components : SList<ComponentEntry> {}

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
            foreach(var c in Components) {
                var component = c.Component;
                foreach(var f in c.Fields) {
                    // Get asset(s) at path
                    UnityEngine.Object asset = null;
                    UnityEngine.Object[] assets = null;
                    switch(f.SubAssetType) {
                        case SubAssetType.FbxChild:
                            assets = Core.Instance.Assets.availableBundles[f.BundleName].AssetBundle.LoadAssetWithSubAssets(f.Path);
                            break;
                        default:
                            asset = Core.Instance.Assets.LoadAssetFromBundle<UnityEngine.Object>(f.BundleName, f.Path);
                            break;
                    }

                    if(asset == null && assets == null) {
                        Log.Info(string.Format("{0}: Restoring reference to vanilla asset failed, asset not found: {1}.{2} = LoadAssetFromBundle(\"{3}\", \"{4}\")", nameof(VanillaAssetReferenceV2), component.GetType().Name, f.Name, f.BundleName, f.Path));
                        continue;
                    }

                    // Get sub-asset by name/subpath
                    switch(f.SubAssetType) {
                        case SubAssetType.FbxChild:
                            foreach(var a in assets) {
                                if(a.name == f.SubPath) {
                                    asset = a;
                                    break;
                                }
                            }
                            break;
                        case SubAssetType.MixerGroup:
                            var mixer = (AudioMixer)asset;
                            asset = mixer.FindMatchingGroups(f.SubPath)[0];
                            break;
                    }
                    
                    if(asset == null) {
                        Log.Info(string.Format("{0}: Restoring reference to vanilla asset failed, sub-asset not found: {1}.{2} = LoadAssetFromBundle(\"{3}\", \"{4}\"); SubAssetType={5}; SubPath={6}", nameof(VanillaAssetReferenceV2), component.GetType().Name, f.Name, f.BundleName, f.Path, f.SubAssetType.ToString(), f.SubPath));
                        continue;
                    }

                    var message = "";

                    try {
                        if(component is Animation && f.Name == "AddClip") {
                            AddAnimationClip(asset, component, f, out message);
                        } else {
                            AssignMember(asset, component, f, out message);
                        }
                    } catch(Exception e) {
                        Log.Info(message + "\nFailed with error:\n" + e.Message + "\n" + e.StackTrace);
                    }
                }
            }
        }

        public static void AssignMember(UnityEngine.Object asset, Component component, FieldEntry f, out string message) {
            // Check for both private and public fields
            var componentType = component.GetType();
            var member = componentType.GetMember(f.Name, UseTheseBindingFlags)[0];

            message = string.Format(
                "{0}: Assigning {1}.{2} = asset {3}:{4} (asset found={5}, field found={6}, asset type={7})",
                nameof(VanillaAssetReferenceV2), componentType.Name, f.PropertyPath, f.BundleName, f.Path,
                asset != null, member != null, asset != null ? asset.GetType().Name : "<not found>"
            );

            if(f.Index >= 0) {
                var collection = member is PropertyInfo p ? p.GetValue(component) : ((FieldInfo)member).GetValue(component);
                collection.GetType().GetProperty("Item").SetValue(collection, asset, new object[] { f.Index });
            } else {
                if(member is PropertyInfo p) {
                    var a = GetComponentFromPrefab(p.PropertyType, asset);
                    p.SetValue(component, a);
                } else if(member is FieldInfo fieldInfo) {
                    var a = GetComponentFromPrefab(fieldInfo.FieldType, asset);
                    fieldInfo.SetValue(component, a);
                } else {
                    Log.Info($"Unexpected member type: {member.MemberType}");
                }
            }

            Object GetComponentFromPrefab(Type FieldOrPropertyType, Object asset) {
                if(asset is GameObject go && FieldOrPropertyType.IsSubclassOf(typeof(Component))) {
                    return go.GetComponent(FieldOrPropertyType);
                } else {
                    return asset;
                }
            }
        }

        public static void AddAnimationClip(UnityEngine.Object asset, Component component, FieldEntry f, out string message) {
            message = 
                $"{nameof(VanillaAssetReferenceV2)}: Animation.AddClip(asset, {asset.name}) " + 
                $"where asset is {f.BundleName}:{f.Path}{(f.SubPath != null ? ":" + f.SubPath : "")} " +
                $"(asset type={(asset != null ? asset.GetType().Name : "<not found>")})";
            (component as Animation).AddClip(asset as AnimationClip, asset.name);
        }
#endif
    }

    [Serializable]
    public class ComponentEntry {
        [SerializeField]
        public Component Component;

        public List<FieldEntry> Fields => fields.items;
        [SerializeReference] private SList_FieldEntry fields = new();
        private class SList_FieldEntry : SList<FieldEntry> {}

        public ComponentEntry() {}
    }

    [Serializable]
    public class FieldEntry {
        [HideInInspector]
        [SerializeField] public bool Enabled = true;
        [HideInInspector]
        [SerializeField] public bool AutoSync = true;
        [SerializeField] public string Name;
        [HideInInspector]
        [SerializeField] public int Index = -1;
        [SerializeField] public string BundleName;
        [SerializeField] public string Path;
        [SerializeField] public SubAssetType SubAssetType = SubAssetType.None;
        [SerializeField] public string SubPath = "";
        public string PropertyPath => Index >= 0 ? $"{Name}[{Index}]" : Name;
        public FieldEntry() {}
    }

    public enum SubAssetType {
        None,
        MixerGroup,
        FbxChild
    }
}
