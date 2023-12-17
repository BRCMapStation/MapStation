using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public class BuiltToy : MonoBehaviour {
        public Toys Toy;
        private Material material;
        private static int GraffitiTextureProperty = Shader.PropertyToID("_Graffiti");

        private void Awake() {
            var renderer = GetComponentInChildren<Renderer>();
            material = renderer.sharedMaterial;
        }

        public void SetGraffiti(GraffitiArt graffiti) {
            material.SetTexture(GraffitiTextureProperty, graffiti.graffitiMaterial.mainTexture);
        }
    }
}
