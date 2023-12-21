using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPI;
using UnityEngine;

namespace Winterland.Common {

    /// <summary>
    /// Replaces the material for this GameObject's renderer component with water.
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class WaterMaterial : MonoBehaviour {
        public enum WaterTypes {
            Pyramid,
            Oasis
        }

        public WaterTypes WaterType;
        private void Awake() {
            Material waterMat;

            switch (WaterType) {
                case WaterTypes.Pyramid:
                    waterMat = AssetAPI.GetMaterial(AssetAPI.MaterialNames.ToonWaterPyramid);
                    break;
                case WaterTypes.Oasis:
                    waterMat = AssetAPI.GetMaterial(AssetAPI.MaterialNames.OasisWater);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(WaterType));
            }

            var renderer = GetComponent<Renderer>();
            renderer.sharedMaterial = waterMat;
        }
    }
}
