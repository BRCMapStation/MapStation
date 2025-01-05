using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace MapStation.Common.Runtime {
    [RequireComponent(typeof(Camera))]
    public class MapStationCameraOverride : MonoBehaviour {
        public static MapStationCameraOverride Instance { get; private set; } = null;
        public bool AlsoAffectPhoneCamera = true;
        private PostProcessLayer _layer;

        private void Awake() {
            Instance = this;
            var cam = GetComponent<Camera>();
            if (cam != null)
                cam.enabled = false;
            _layer = GetComponent<PostProcessLayer>();
            if (_layer != null)
                _layer.enabled = false;
            var audioListener = GetComponent<AudioListener>();
            if (audioListener != null)
                audioListener.enabled = false;
        }

        private void OnDestroy() {
            Instance = null;
        }

        public void ApplyToCamera(Camera camera) {
            if (camera == null) return;
            if (_layer != null) {
                var newLayer = camera.gameObject.GetComponent<PostProcessLayer>();
                if (newLayer == null)
                    newLayer = camera.gameObject.AddComponent<PostProcessLayer>();
                newLayer.finalBlitToCameraTarget = _layer.finalBlitToCameraTarget;
                newLayer.stopNaNPropagation = _layer.stopNaNPropagation;
                newLayer.volumeTrigger = camera.transform;
                newLayer.volumeLayer = _layer.volumeLayer;
                newLayer.antialiasingMode = _layer.antialiasingMode;
                newLayer.fastApproximateAntialiasing = _layer.fastApproximateAntialiasing;
                newLayer.subpixelMorphologicalAntialiasing = _layer.subpixelMorphologicalAntialiasing;
                newLayer.temporalAntialiasing = _layer.temporalAntialiasing;
                var resourcesField = typeof(PostProcessLayer).GetField("m_Resources", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                resourcesField.SetValue(newLayer, resourcesField.GetValue(_layer));
            }
        }
    }
}
