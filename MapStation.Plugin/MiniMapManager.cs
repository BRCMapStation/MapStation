using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapStation.Common;
using Reptile;
using UnityEngine;

namespace MapStation.Plugin {
    public static class MiniMapManager {
        public static bool TryCreateMapForCustomStage(BaseMapDatabaseEntry mapEntry, out Map map) { var assets = Core.Instance.Assets;
            var miniMapPrefab = assets.LoadAssetFromBundle<GameObject>(mapEntry.AssetsBundleName, "MiniMap.prefab");
            if (miniMapPrefab == null) {
                map = null;
                return false;
            }
            var minimap = ScriptableObject.CreateInstance<Map>();
            minimap.m_MapObject = miniMapPrefab;
            minimap.m_ScaleFactor = 1f;
            minimap.m_PositionOffset = Vector3.zero;
            minimap.m_EulerOffset = Vector3.zero;
            minimap.mapMaterial = Mapcontroller.Instance.pyramidMap.mapMaterial;
            map = minimap;
            ProcessCustomMiniMapPrefab(map);
            minimap.mapMaterial.SetFloat("_AnchorOffset", -5000f);
            minimap.mapMaterial.SetFloat("_AnchorScale", 0.035f);
            return true;
        }

        private static void ProcessCustomMiniMapPrefab(Map map) {
            var renderers = map.m_MapObject.GetComponentsInChildren<Renderer>();
            foreach(var renderer in renderers) {
                if (renderer.sharedMaterial != null && renderer.sharedMaterial.shader.name == "BRC/Minimap") {
                    map.mapMaterial = renderer.sharedMaterial;
                }
                renderer.gameObject.layer = Layers.Minimap;
            }
            var sortedRenderers = renderers.OrderBy(renderer => renderer.gameObject.transform.position.y).ToArray();
            for(var i = 0; i < sortedRenderers.Length; i++) {
                var renderer = sortedRenderers[i];
                renderer.sortingOrder = i;
            }
        }
    }
}
