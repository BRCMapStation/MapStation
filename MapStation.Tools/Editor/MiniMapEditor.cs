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
