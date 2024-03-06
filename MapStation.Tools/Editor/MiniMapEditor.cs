using MapStation.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace MapStation.Tools.Editor {
    

    public static class MiniMapEditor {

        [MenuItem("Assets/Create/" + UIConstants.menuLabel + "/MiniMap", false)]
        public static void CreateMiniMap() {
            var prefabSource = Path.Combine(ToolAssetConstants.NewMapTemplatePath, "MiniMap.prefab");
            var materialSource = Path.Combine(ToolAssetConstants.NewMapTemplatePath, "MiniMapMaterial.mat");

            var currentfolder = GetCurrentFolder();
            var prefabDest = Path.Combine(currentfolder, "MiniMap.prefab");
            var materialDest = Path.Combine(currentfolder, "MiniMapMaterial.mat");

            if (File.Exists(prefabDest)) {
                Debug.LogError("Can't create MiniMap - a \"MiniMap.prefab\" Asset already exists in this directory.");
                return;
            }

            if (File.Exists(prefabDest)) {
                Debug.LogError("Can't create MiniMap - a \"MiniMapMaterial.mat\" Asset already exists in this directory.");
                return;
            }

            AssetDatabase.CopyAsset(prefabSource, prefabDest);
            AssetDatabase.CopyAsset(materialSource, materialDest);
            AssetDatabase.SaveAssets();

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabDest);
            FixMiniMapReferences(prefab);
            Selection.activeObject = prefab;
        }

        private static string GetCurrentFolder() {
            var location = "Assets";
            var activeObject = Selection.activeObject;
            if (activeObject != null) {
                location = AssetDatabase.GetAssetPath(activeObject);
            }
            if (File.Exists(location))
                location = Path.GetDirectoryName(location);
            return location;
        }

        public static void FixMiniMapReferences(GameObject miniMapPrefab) {
            var prefabPath = AssetDatabase.GetAssetPath(miniMapPrefab);
            var prefabFolder = Path.GetDirectoryName(prefabPath);

            var miniMapMaterialPath = Path.Combine(prefabFolder, "MiniMapMaterial.mat");
            var miniMapMaterial = AssetDatabase.LoadAssetAtPath<Material>(miniMapMaterialPath);

            var miniMapProperties = miniMapPrefab.GetComponent<MiniMapProperties>();
            if (miniMapProperties != null) {
                miniMapProperties.MapMaterial = miniMapMaterial;
                EditorUtility.SetDirty(miniMapProperties);
            }

            var renderers = miniMapPrefab.GetComponentsInChildren<Renderer>();

            foreach(var renderer in renderers) {
                if (renderer.sharedMaterial.name == "MiniMapMaterial") {
                    renderer.sharedMaterial = miniMapMaterial;
                    EditorUtility.SetDirty(renderer);
                }
            }

            AssetDatabase.SaveAssets();
        }
    }
}
