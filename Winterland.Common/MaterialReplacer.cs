using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public class MaterialReplacer : MonoBehaviour {
        public string NameOfMaterialToReplace = null;
        public Material ReplacementMaterial = null;

        private void Awake() {
            var renderers = FindObjectsOfType<Renderer>();
            foreach(var renderer in renderers) {
                var newMaterials = new Material[renderer.sharedMaterials.Length];
                for(var i = 0; i < renderer.sharedMaterials.Length; i++) {
                    var material = renderer.sharedMaterials[i];
                    if (material.name == NameOfMaterialToReplace)
                        newMaterials[i] = ReplacementMaterial;
                    else
                        newMaterials[i] = material;
                }
                renderer.sharedMaterials = newMaterials;
            }
        }
    }
}
