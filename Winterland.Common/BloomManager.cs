using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public class BloomManager : MonoBehaviour {
        public static BloomManager Instance = null;
        public Material BloomPassMaterial = null;
        public Material ScreenPassMaterial = null;
        public int ResolutionDivide = 4;
        [HideInInspector]
        public RenderTexture BloomRT = null;
        private int screenPassBloomTextureProperty = Shader.PropertyToID("_BloomTex");

        private void Awake() {
            Instance = this;
            var core = Core.Instance;
            MakeBloomRT(core.currentScreenWidth, core.currentScreenHeight);
            Core.OnScreenSizeChanged += ScreenSizeChanged;
        }

        private void Start() {
            AddBloomToCamera(GameplayCamera.instance.gameObject);
        }

        private void AddBloomToCamera(GameObject camera) {
            var existingBloomEffect = camera.GetComponent<CameraBloomEffect>();
            if (existingBloomEffect != null) return;
            camera.AddComponent<CameraBloomEffect>();
        }

        private void OnDestroy() {
            Core.OnScreenSizeChanged -= ScreenSizeChanged;
            BloomRT.Release();
        }

        private void ScreenSizeChanged(int newWidth, int newHeight) {
            MakeBloomRT(newWidth, newHeight);
        }

        private void MakeBloomRT(int screenWidth, int screenHeight) {
            if (BloomRT != null)
                BloomRT.Release();
            var width = screenWidth / ResolutionDivide;
            var height = screenHeight / ResolutionDivide;
            BloomRT = new RenderTexture(width, height, 24);
            ScreenPassMaterial.SetTexture(screenPassBloomTextureProperty, BloomRT);
        }
    }
}
