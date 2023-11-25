using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using UnityEngine;

namespace Winterland.Common {
    [ExecuteAlways]
    public class AmbientOverride : MonoBehaviour {
        public static AmbientOverride Instance = null;
        [Header("Skybox texture. Leave this at null to keep the original stage skybox.")]
        public Texture Skybox = null;
        public Color LightColor = Color.white;
        public Color ShadowColor = Color.black;

        private void Update() {
            if (Application.isEditor) {
                Shader.SetGlobalColor("LightColor", LightColor);
                Shader.SetGlobalColor("ShadowColor", ShadowColor);
            }
        }

        private void Awake() {
            Instance = this;
            if (!Application.isEditor) {
                ReptileAwake();
            }

            void ReptileAwake() {

                if (Skybox != null)
                    RenderSettings.skybox.mainTexture = Skybox;

                var sun = FindObjectOfType<AmbientManager>();
                if (sun != null) {
                    sun.transform.rotation = transform.rotation;
                }

                var myLite = GetComponent<Light>();
                Destroy(myLite);
            }
        }

        private void OnDestroy() {
            Instance = null;
        }
    }
}
