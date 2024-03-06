using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapStation.Common;
using Reptile;
using UnityEngine;

namespace MapStation.Plugin {
    /// <summary>
    /// Manages Mini Maps for custom stages.
    /// </summary>
    public static class MiniMapManager {

        // Creates a Map ScriptableObject for a custom stage.
        public static bool TryCreateMapForCustomStage(BaseMapDatabaseEntry mapEntry, out Map map) { var assets = Core.Instance.Assets;
            var miniMapPrefab = assets.LoadAssetFromBundle<GameObject>(mapEntry.AssetsBundleName, "MiniMap.prefab");
            if (miniMapPrefab == null) {
                map = null;
                return false;
            }
            var miniMapProperties = miniMapPrefab.GetComponent<MiniMapProperties>();
            if (miniMapProperties == null) {
                miniMapProperties = miniMapPrefab.AddComponent<MiniMapProperties>();
            }
            var minimap = ScriptableObject.CreateInstance<Map>();
            minimap.m_MapObject = miniMapPrefab;
            minimap.m_ScaleFactor = 1f;
            minimap.m_PositionOffset = Vector3.zero;
            minimap.m_EulerOffset = Vector3.zero;
            minimap.mapMaterial = Mapcontroller.Instance.pyramidMap.mapMaterial;
            map = minimap;
            ProcessCustomMiniMapPrefab(map, miniMapProperties);
            if (miniMapProperties.MapMaterial != null)
                minimap.mapMaterial = miniMapProperties.MapMaterial;

            // Account for map controller transform in the map material shader.
            minimap.mapMaterial.SetFloat("_AnchorOffset", -5000f);
            minimap.mapMaterial.SetFloat("_AnchorScale", 0.035f);
            return true;
        }

        // Sets appropriate layers.
        private static void ProcessCustomMiniMapPrefab(Map map, MiniMapProperties properties) {
            var renderers = map.m_MapObject.GetComponentsInChildren<Renderer>();
            foreach(var renderer in renderers)
                renderer.gameObject.layer = Layers.Minimap;
        }
    }
}
