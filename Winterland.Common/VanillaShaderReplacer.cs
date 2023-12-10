using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CommonAPI;

namespace Winterland.Common {
    public class VanillaShaderReplacer : MonoBehaviour {
#if !UNITY_EDITOR
        private void Awake() {
            var renderers = GetComponentsInChildren<Renderer>();
            foreach(var renderer in renderers) {
                var materials = renderer.sharedMaterials;
                foreach(var material in materials) {
                    if (material.shader.name == "BRC/Ambient Character")
                        material.shader = AssetAPI.GetShader(AssetAPI.ShaderNames.AmbientCharacter);
                }
            }
        }
#endif
    }
}
