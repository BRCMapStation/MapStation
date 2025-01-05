using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MapStation.Common.Runtime {
    public class LightingChanger : MonoBehaviour {
        [Header("Changes the sun lighting when enabled.")]
        [Header("This GameObject's Transform will be applied to the Sun.")]
        [Header("Light Settings")]
        public Color LightColor = Color.white;
        [Header("BRC Shading Settings")]
        public Color EnvLightColor = Color.white;
        public Color EnvShadowColor = Color.black;

        private static Quaternion OldRotation = Quaternion.identity;
        private static Color OldLightColor = Color.white;
        private static Color OldEnvLightColor = Color.white;
        private static Color OldEnvShadowColor = Color.black;

        private static LightingChanger CurrentLightingChanger = null;

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

        private AmbientManager GetAmbientManager() {
            AmbientManager ambientManager = null;
            var ambientManagers = FindObjectsOfType<AmbientManager>();
            foreach (var manager in ambientManagers) {
                if (manager.gameObject.scene == gameObject.scene) {
                    ambientManager = manager;
                    break;
                }
            }
            return ambientManager;
        }

        private void OnEnable() {
            var ambientManager = GetAmbientManager();
            if (CurrentLightingChanger == null) {
                OldRotation = ambientManager.transform.rotation;
                OldLightColor = ambientManager.GetComponent<Light>().color;
                OldEnvLightColor = ambientManager.AmbientEnvLight;
                OldEnvShadowColor = ambientManager.AmbientEnvShadow;
            }
            CurrentLightingChanger = this;
            ambientManager.GetComponent<Light>().color = LightColor;
            ambientManager.transform.rotation = transform.rotation;
            ambientManager.AmbientEnvLight = EnvLightColor;
            ambientManager.AmbientEnvShadow = EnvShadowColor;
            if (ambientManager.curAmbientTrigger == null) {
                ambientManager.currentDefaultAmbientEnvLight = EnvLightColor;
                ambientManager.currentDefaultAmbientEnvShadow = EnvShadowColor;
                ambientManager.curLight = EnvLightColor;
                ambientManager.curShadow = EnvShadowColor;
                ambientManager.RevertAmbient(0f);
            }
        }

        private void OnDisable() {
            if (CurrentLightingChanger != this) return;
            CurrentLightingChanger = null;
            var ambientManager = GetAmbientManager();
            ambientManager.GetComponent<Light>().color = OldLightColor;
            ambientManager.transform.rotation = OldRotation;
            ambientManager.AmbientEnvLight = OldEnvLightColor;
            ambientManager.AmbientEnvShadow = OldEnvShadowColor;
            if (ambientManager.curAmbientTrigger == null) {
                ambientManager.currentDefaultAmbientEnvLight = OldEnvLightColor;
                ambientManager.currentDefaultAmbientEnvShadow = OldEnvShadowColor;
                ambientManager.curLight = OldEnvLightColor;
                ambientManager.curShadow = OldEnvShadowColor;
                ambientManager.RevertAmbient(0f);
            }
        }
#endif
    }
}
