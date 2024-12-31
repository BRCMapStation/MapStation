using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MapStation.Common.Runtime {
    public class SkyboxSwitcher : MonoBehaviour {
        [Header("Changes the skybox when enabled.")]
        public Material Skybox;
        private Material _oldSkybox;
#if BEPINEX
        private void Awake() {
            StageManager.OnStageInitialized += StageInitialized;
        }

        private void OnDestroy() {
            StageManager.OnStageInitialized -= StageInitialized;
        }

        private void StageInitialized() {
            if (isActiveAndEnabled)
                OnEnable();
        }
#endif

        private void OnEnable() {
            _oldSkybox = RenderSettings.skybox;
            RenderSettings.skybox = Skybox;
        }

        private void OnDisable() {
            if (RenderSettings.skybox == Skybox)
                RenderSettings.skybox = _oldSkybox;
        }
    }
}
