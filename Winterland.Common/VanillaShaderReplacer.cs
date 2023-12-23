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
            var renderers = GetComponentsInChildren<Renderer>(true);
            foreach(var renderer in renderers) {
                var materials = renderer.sharedMaterials;
                foreach(var material in materials) {
                    switch (material.shader.name) {
                        case "BRC/Ambient Character":
                            material.shader = AssetAPI.GetShader(AssetAPI.ShaderNames.AmbientCharacter);
                            break;
                        case "BRC/Ambient Environment":
                            material.shader = AssetAPI.GetShader(AssetAPI.ShaderNames.AmbientEnvironment);
                            break;
                        case "BRC/Ambient Environment Cutout":
                            material.shader = AssetAPI.GetShader(AssetAPI.ShaderNames.AmbientEnvironmentCutout);
                            break;
                        case "BRC/Ambient Environment Transparent":
                            material.shader = AssetAPI.GetShader(AssetAPI.ShaderNames.AmbientEnvironmentTransparent);
                            break;
                    }
                }
            }
        }
#endif
    }
}
