using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using UnityEngine;
using UnityEngine.Video;

namespace Winterland.Common {
    [ExecuteAlways]
    public class AmbientOverride : MonoBehaviour {
        public static AmbientOverride Instance = null;
        public bool Night = false;
        [HideInInspector]
        public Color AdditiveSkyColor = Color.black;
        [Header("How fast additive sky colors fade to black (Fireworks)")]
        public float AdditiveSkyColorLerpSpeed = 5f;
        [HideInInspector]
        public bool DayNightCycleModEnabled = true;
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
#if !UNITY_EDITOR
        private AmbientManager sun;
#endif

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

                AdditiveSkyColor = Vector4.Lerp(AdditiveSkyColor, Color.black, AdditiveSkyColorLerpSpeed * Core.dt);

                if (CurrentAmbientTrigger == null) {
                    var additiveAverage = (AdditiveSkyColor.r + AdditiveSkyColor.g + AdditiveSkyColor.b) / 3f;
                    var shadowMultiplier = -(additiveAverage * 0.5f) + 1f;
                    CurrentShadowColor *= shadowMultiplier;
                    CurrentLightColor *= shadowMultiplier;
                    CurrentLightColor += AdditiveSkyColor;
                }
            }
        }

        public void AddSkyLight(Color color) {
            AdditiveSkyColor += color * 0.7f;
        }

#if !UNITY_EDITOR
        private void LateUpdate() {
            if (sun == null) return;
            if (DayNightCycleModEnabled) return;
            var light = sun.GetComponent<Light>();
            light.color = Color.white;
            light.shadowStrength = 1f;
        }
#endif

        public void TransitionAmbient(AmbientOverrideTrigger trigger) {
            if (CurrentAmbientTrigger != trigger) {
                CurrentAmbientTrigger = trigger;
                currentTimer = 0f;
                currentTransitionDuration = trigger.TransitionDuration;
                oldLightColor = CurrentLightColor;
                oldShadowColor = CurrentShadowColor;
                DayNightCycleModEnabled = trigger.EnableDayLightCycleMod;
            }
        }

        public void StopAmbient(AmbientOverrideTrigger trigger) {
            if (trigger == CurrentAmbientTrigger) {
                CurrentAmbientTrigger = null;
                currentTimer = 0f;
                currentTransitionDuration = trigger.TransitionDuration;
                oldLightColor = CurrentLightColor;
                oldShadowColor = CurrentShadowColor;
                DayNightCycleModEnabled = true;
            }
        }

        private void Awake() {
            Instance = this;
#if !UNITY_EDITOR
            oldLightColor = LightColor;
            oldShadowColor = ShadowColor;

            if (Skybox != null)
                RenderSettings.skybox.mainTexture = Skybox;

            sun = FindObjectOfType<AmbientManager>();
            if (sun != null) {
                sun.transform.rotation = transform.rotation;

                if (Night) {
                    var lens = sun.GetComponent<LensFlare>();
                    var glare = sun.GetComponent<SunGlare>();
                    var flare = sun.GetComponent<SunFlareGPU>();
                    Destroy(lens);
                    Destroy(glare);
                    Destroy(flare);
                }
            }

            var sunLight = sun.GetComponent<Light>();
            var myLite = GetComponent<Light>();
            sunLight.color = myLite.color;
            if (myLite != null)
                Destroy(myLite);
#endif
        }

        private void OnDestroy() {
            Instance = null;
        }
    }
}
