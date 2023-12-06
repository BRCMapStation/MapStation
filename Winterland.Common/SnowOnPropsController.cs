using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    /// <summary>
    /// Puts snow on dynamic level objects like cars... maybe junk too. maybe not. who knows!
    /// </summary>
    public class SnowOnPropsController : MonoBehaviour {
        [SerializeField]
        private Material snowMaterial = null;
        [SerializeField]
        private float variation = 0.15f;
        private List<UnityEngine.Object> managedResources = null;
        private void Awake() {
            var strengthProperty = Shader.PropertyToID("_SnowDetailStrength");
            var snowDetailProperty = Shader.PropertyToID("_SnowDetail");
            var defaultStrength = snowMaterial.GetFloat(strengthProperty);
            managedResources = new();

            var cars = FindObjectsOfType<Car>();
            foreach(var car in cars) {
                var handler = car.GetComponentInParent<CarsMoveHandler>(true);
                if (handler == null)
                    continue;
                var meshes = car.GetComponentsInChildren<MeshRenderer>(true);
                foreach(var mesh in meshes) {
                    if (mesh.gameObject.name.StartsWith("Car_") && !mesh.gameObject.name.EndsWith("_Col")) {
                        var strengthDelta = UnityEngine.Random.Range(-variation, variation);
                        var strength = Mathf.Clamp(defaultStrength + strengthDelta, 0f, 1f);
                        mesh.sharedMaterial = MakeCarMaterial(mesh.sharedMaterial);
                        mesh.sharedMaterial.SetFloat(strengthProperty, strength);
                        var offset = mesh.sharedMaterial.GetTextureOffset(snowDetailProperty);
                        offset.x += UnityEngine.Random.Range(0f, 200f);
                        offset.y += UnityEngine.Random.Range(0f, 200f);
                        mesh.sharedMaterial.SetTextureOffset(snowDetailProperty, offset);
                        break;
                    }
                }
            }
        }

        private void OnDestroy() {
            foreach(var resource in managedResources) {
                UnityEngine.Object.DestroyImmediate(resource);
            }
        }

        private Material MakeCarMaterial(Material carMaterial) {
            var mat = new Material(snowMaterial);
            mat.mainTexture = carMaterial.mainTexture;
            managedResources.Add(mat);
            return mat;
        }
    }
}
