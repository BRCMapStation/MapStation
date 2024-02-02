using UnityEngine;
using UnityEditor;
using System.Reflection;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace MapStation.Common.VanillaAssets {
    [CustomEditor(typeof(VanillaAssetReference))]
    public class VanillaAssetReferenceEditor : UnityEditor.Editor {
        VanillaAssetReference t { get => target as VanillaAssetReference; }

        public override void OnInspectorGUI() {
            EditorGUILayout.HelpBox(
                "Repairs references to base game assets by re-assigning them at runtime.\n" +
                "References from our asset bundles to vanilla game asset bundles have the wrong IDs and are null at runtime unless we fix them.\n" +
                "This component stores the assetbundle name and asset path so it can retrieve the assets at runtime.",
                MessageType.Info);
            base.OnInspectorGUI();
            if(GUILayout.Button("Add field")) {
                var menu = new GenericMenu();
                var items = new List<string>();
                var members = t.component.GetType().GetMembers(VanillaAssetReference.UseTheseBindingFlags).Where(m =>
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
                        Name = member.Name,
                        Index = -1
                    };
                    menu.AddItem(new GUIContent(item.PropertyPath), false, onFieldSelected, item);
                }
                menu.ShowAsContext();
            }
        }

        private struct MenuItemValue {
            public string Name;
            public int Index;
            public string PropertyPath { get => Index >= 0 ? Name + "[" + Index + "]" : Name; }
        }

        private void onFieldSelected(object selected) {
            var s = (MenuItemValue)selected;
            
            var f = t.component.GetType().GetMember(s.Name, 
                VanillaAssetReference.UseTheseBindingFlags
            )[0].ToManipulator();
            var value = f.Get(t.component);
            if(s.Index >= 0) {
                if(value is List<object> l) {
                    value = l[s.Index];
                } else if(value is object[] a) {
                    value = a[s.Index];
                } else {
                    throw new Exception("Unexpected collection type");
                }
            }

            if(value is not UnityEngine.Object) {
                Debug.LogError(string.Format("Field {0} does not refer to an asset.", s.PropertyPath));
            }
            var path = AssetDatabase.GetAssetPath(value as UnityEngine.Object);
            if(path == null || path == "") {
                Debug.LogError(string.Format("Field {0} does not refer to an asset.", s.PropertyPath));
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
            
            Undo.RecordObject(t, "Add field");
            t.fields.Add(String.Format("{0}={1}:{2}", s.PropertyPath, bundle, path));
        }
    }

    class MemberCompareByName : IComparer<MemberInfo> {
        public int Compare(MemberInfo a, MemberInfo b) {
            return a.Name.CompareTo(b.Name);
        }
    }
}
