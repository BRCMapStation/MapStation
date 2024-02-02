using UnityEngine;
using UnityEditor;
using System.Reflection;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Audio;

namespace MapStation.Common.VanillaAssets {
    public static class Configuration {
        /// <summary>
        /// Prefixes to be stripped from asset paths.
        /// Allows you to nest Reptile's vanilla assets within a subdirectory of
        /// your project.
        /// </summary>
        public static List<string> StripPathPrefixes = new() {
            "Packages/com.brcmapstation.tools/Assets/ReptileAssets/"
        };
    }

    [CustomEditor(typeof(VanillaAssetReferenceV2))]
    public class VanillaAssetReferenceV2Editor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            EditorGUILayout.HelpBox(
                "Repairs references to base game assets by re-assigning them at runtime.\n" +
                "References from our asset bundles to vanilla game asset bundles have the wrong IDs and are null at runtime unless we fix them.\n" +
                "This component stores the assetbundle name and asset path so it can retrieve the assets at runtime.",
                MessageType.Info);
            base.OnInspectorGUI();
        }
    }

    [CustomPropertyDrawer(typeof(ComponentEntry))]
    public class VanillaAssetReferenceV2ComponentEntryDrawer : PropertyDrawer {

        private VanillaAssetReferenceV2 getOwner(SerializedProperty property) {
            return (VanillaAssetReferenceV2)property.serializedObject.targetObject;
        }

        private ComponentEntry getValue(SerializedProperty property) {
            return (ComponentEntry)property.managedReferenceValue;
        }

        private const int ButtonHeight = 20;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, label, true) + ButtonHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            VanillaAssetReferenceV2 owner = getOwner(property);
            ComponentEntry t = getValue(property);

            var positionAbove = new Rect(position.x, position.y, position.width, position.height - ButtonHeight);
            var positionBelow = new Rect(position.x, position.yMax - ButtonHeight, position.width, ButtonHeight);

            // Default inspector
            EditorGUI.PropertyField(positionAbove, property, true);

            if(GUI.Button(positionBelow, "Add field")) {
                var menu = new GenericMenu();

                var members = t.Component.GetType().GetMembers(VanillaAssetReference.UseTheseBindingFlags).Where(m =>
                    m.IsAccessor() &&
                        m.DeclaringType != typeof(MonoBehaviour) &&
                        m.DeclaringType != typeof(Behaviour) &&
                        m.DeclaringType != typeof(Component) &&
                        m.DeclaringType != typeof(UnityEngine.Object)
                ).OrderBy(m => m.Name).ToList();

                foreach(var member in members) {
                    Type accessedType = member.GetAccessorType();

                    // TOO MUCH WORK TO SUPPORT COLLECTIONS
                    // I only care about MeshRenderer.sharedMaterials which has
                    // a singular alternative sharedMaterial

                    // var f = accessedType.GetMember("Count");
                    // if(f.Length == 0) f = accessedType.GetMember("Length");
                    // if(f.Length != 0) {
                    //     var _l = f[0].ToManipulator().Get(member.ToManipulator().Get(t.component));
                    //     if(_l is not null && _l is int l && l < 10) {
                    //         for(var i = 0; i < l; i++) {
                    //             var label = member.Name + "[" + i + "]";
                    //             var indexItem = new MenuItemValue {
                    //                 Name = member.Name,
                    //                 Index = i
                    //             };
                    //             menu.AddItem(new GUIContent(indexItem.Label), false, onFieldSelected, indexItem);
                    //         }
                    //         continue;
                    //     }
                    // }

                    var item = new MenuItemValue {
                        Owner = owner,
                        ComponentEntry = t,
                        Name = member.Name,
                        Index = -1
                    };
                    menu.AddItem(new GUIContent(item.PropertyPath), false, onFieldSelected, item);
                }
                menu.ShowAsContext();
            }
        }

        private struct MenuItemValue {
            public VanillaAssetReferenceV2 Owner;
            public ComponentEntry ComponentEntry;
            public string Name;
            public int Index;
            public readonly string PropertyPath => Index >= 0 ? Name + "[" + Index + "]" : Name;
        }

        private void onFieldSelected(object selected_) {
            var selected = (MenuItemValue)selected_;
            var component = selected.ComponentEntry.Component;

            var member = component.GetType().GetMember(selected.Name, 
                VanillaAssetReference.UseTheseBindingFlags
            )[0].ToManipulator();
            var value = member.Get(component);
            if(selected.Index >= 0) {
                if(value is List<object> l) {
                    value = l[selected.Index];
                } else if(value is object[] a) {
                    value = a[selected.Index];
                } else {
                    throw new Exception("Unexpected collection type");
                }
            }

            if(value is not UnityEngine.Object) {
                Debug.LogError(string.Format("Field {0} does not refer to an asset.", selected.PropertyPath));
            }
            var path = AssetDatabase.GetAssetPath(value as UnityEngine.Object);
            if(path == null || path == "") {
                Debug.LogError(string.Format("Field {0} does not refer to an asset.", selected.PropertyPath));
                return;
            }
            var bundle = AssetImporter.GetAtPath(path).assetBundleName;
            if (bundle == null || bundle == "") {
                Debug.LogError("Referenced asset is not assigned to an assetbundle.");
                return;
            }

            foreach(var prefix in Configuration.StripPathPrefixes) {
                if(path.IndexOf(prefix) == 0) {
                    path = "Assets/" + path[(prefix.Length)..];
                    break;
                }
            }

            var type = SubAssetType.None;
            var SubPath = "";
            if(value is AudioMixerGroup audioMixerGroup) {
                type = SubAssetType.MixerGroup;
                SubPath = audioMixerGroup.name;
            }

            // "Complete" Necessary to record nested objects which are [SerializeReference]
            Undo.RegisterCompleteObjectUndo(selected.Owner, $"Add Field to {nameof(VanillaAssetReferenceV2)}");
            selected.ComponentEntry.Fields.Add(new () {
                Name = selected.Name,
                BundleName = bundle,
                Path = path,
                Index = selected.Index,
                SubAssetType = type,
                SubPath = SubPath
            });
        }
    }

    class MemberInfoCompareByName : IComparer<MemberInfo> {
        public int Compare(MemberInfo a, MemberInfo b) {
            return a.Name.CompareTo(b.Name);
        }
    }
}
