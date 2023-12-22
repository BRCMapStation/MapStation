using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CommonAPI;

namespace Winterland.Common {
    public class MaterialReplacer : MonoBehaviour {
        public string NameOfMaterialToReplace = null;
        public Material ReplacementMaterial = null;
        public bool UseBRCShader = false;
        public AssetAPI.ShaderNames BRCShaderName = AssetAPI.ShaderNames.AmbientEnvironment;

        private void Start() {
            if (UseBRCShader)
                ReplacementMaterial.shader = AssetAPI.GetShader(BRCShaderName);
            var renderers = FindObjectsOfType<Renderer>();
            foreach(var renderer in renderers) {
                if (!renderer.sharedMaterials.Any((x) => (x != null && x.name == NameOfMaterialToReplace)))
                    continue;
                var newMaterials = new Material[renderer.sharedMaterials.Length];
                for(var i = 0; i < renderer.sharedMaterials.Length; i++) {
                    var material = renderer.sharedMaterials[i];
                    if (material != null && material.name == NameOfMaterialToReplace)
                        newMaterials[i] = ReplacementMaterial;
                    else
                        newMaterials[i] = material;
                }
                renderer.sharedMaterials = newMaterials;
            }
        }
    }
}
