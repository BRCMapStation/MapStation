using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MapStation.Common.Runtime {
    public class FogChanger : MonoBehaviour {
        [Header("Changes fog settings when enabled.")]
        public bool FogEnabled = false;
        public Color Color = Color.grey;
        [Header("Linear Fog Settings")]
        public float Start = 0f;
        public float End = 300f;
        [Header("Exponential Fog Settings")]
        public float Density = 0.01f;

        private static bool OldFogEnabled = false;
        private static Color OldFogColor = Color.grey;
        private static float OldFogStart = 0f;
        private static float OldFogEnd = 300f;
        private static float OldFogDensity = 0.01f;

        private static FogChanger CurrentFogChanger = null;

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

        private void OnEnable() {
            if (CurrentFogChanger == null) {
                OldFogEnabled = RenderSettings.fog;
                OldFogColor = RenderSettings.fogColor;
                OldFogStart = RenderSettings.fogStartDistance;
                OldFogEnd = RenderSettings.fogEndDistance;
                OldFogDensity = RenderSettings.fogDensity;
            }
            CurrentFogChanger = this;
            RenderSettings.fog = FogEnabled;
            RenderSettings.fogColor = Color;
            RenderSettings.fogDensity = Density;
            RenderSettings.fogStartDistance = Start;
            RenderSettings.fogEndDistance = End;
        }

        private void OnDisable() {
            if (CurrentFogChanger != this) return;
            CurrentFogChanger = null;
            RenderSettings.fog = OldFogEnabled;
            RenderSettings.fogColor = OldFogColor;
            RenderSettings.fogDensity = OldFogDensity;
            RenderSettings.fogStartDistance = OldFogStart;
            RenderSettings.fogEndDistance = OldFogEnd;
        }
#endif
    }
}
