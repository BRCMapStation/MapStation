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
        [Header("Skybox texture. Leave this set to nothing to keep the original stage skybox.")]
        public Texture Skybox = null;
        public Color LightColor = Color.white;
        public Color ShadowColor = Color.black;
        [HideInInspector]
        public Color CurrentLightColor = Color.white;
        [HideInInspector]
        public Color CurrentShadowColor = Color.black;
        [HideInInspector]
        public AmbientOverrideTrigger CurrentAmbientTrigger = null;
        private Color oldLightColor = Color.white;
        private Color oldShadowColor = Color.black;
        private float currentTimer = 0f;
        private float currentTransitionDuration = 1f;

        private void Update() {
            if (Application.isEditor) {
                Shader.SetGlobalColor("LightColor", LightColor);
                Shader.SetGlobalColor("ShadowColor", ShadowColor);
            }
            else {
                ReptileUpdate();
            }
            
            void ReptileUpdate() {
                currentTimer += Core.dt;
                if (currentTimer > currentTransitionDuration)
                    currentTimer = currentTransitionDuration;
                var progress = 1f;
                if (currentTransitionDuration > 0f)
                    progress = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f).Evaluate(currentTimer / currentTransitionDuration);
                var targetLightColor = LightColor;
                var targetShadowColor = ShadowColor;
                if (CurrentAmbientTrigger != null) {
                    targetLightColor = CurrentAmbientTrigger.LightColor;
                    targetShadowColor = CurrentAmbientTrigger.ShadowColor;
                }
                CurrentLightColor = Vector4.Lerp(oldLightColor, targetLightColor, progress);
                CurrentShadowColor = Vector4.Lerp(oldShadowColor, targetShadowColor, progress);
            }
        }

        public void TransitionAmbient(AmbientOverrideTrigger trigger) {
            if (CurrentAmbientTrigger != trigger) {
                CurrentAmbientTrigger = trigger;
                currentTimer = 0f;
                currentTransitionDuration = trigger.TransitionDuration;
                oldLightColor = CurrentLightColor;
                oldShadowColor = CurrentShadowColor;
            }
        }

        public void StopAmbient(AmbientOverrideTrigger trigger) {
            if (trigger == CurrentAmbientTrigger) {
                CurrentAmbientTrigger = null;
                currentTimer = 0f;
                currentTransitionDuration = trigger.TransitionDuration;
                oldLightColor = CurrentLightColor;
                oldShadowColor = CurrentShadowColor;
            }
        }

        private void Awake() {
            Instance = this;
            if (!Application.isEditor) {
                ReptileAwake();
            }

            void ReptileAwake() {

                oldLightColor = LightColor;
                oldShadowColor = ShadowColor;

                if (Skybox != null)
                    RenderSettings.skybox.mainTexture = Skybox;

                var sun = FindObjectOfType<AmbientManager>();
                if (sun != null) {
                    sun.transform.rotation = transform.rotation;
                }

                var myLite = GetComponent<Light>();
                if (myLite != null)
                    Destroy(myLite);
            }
        }

        private void OnDestroy() {
            Instance = null;
        }
    }
}
