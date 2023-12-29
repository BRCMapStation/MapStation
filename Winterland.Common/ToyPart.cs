using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    [ExecuteAlways]
    public class ToyPart : MonoBehaviour {
        public ToyLine Line { get; private set; }
        public AudioClip PickUpAudioClip = null;
        private MaterialPropertyBlock propertyBlock = null;

        public void Respawn() {
            gameObject.SetActive(true);
        }

        public void Collect(Player player) {
            var winterPlayer = WinterPlayer.Get(player);
            if (winterPlayer.CurrentToyLine != Line)
                winterPlayer.CollectedToyParts = 0;
            winterPlayer.CurrentToyLine = Line;
            winterPlayer.CollectedToyParts++;
            if (WinterUI.Instance != null && winterPlayer.Local) {
                var toyLineUI = WinterUI.Instance.ToyLineUI;
                toyLineUI.Visible = true;
                toyLineUI.SetCounter(winterPlayer.CollectedToyParts, Line.ToyParts.Length);
            }
            gameObject.SetActive(false);
            if (PickUpAudioClip != null) {
                var audioManager = Core.Instance.AudioManager;
                audioManager.PlayNonloopingSfx(audioManager.audioSources[3], PickUpAudioClip, audioManager.mixerGroups[3], 0f);
            }
        }

        private void Awake() {
            Initialize();
        }

        private void OnDestroy() {
            propertyBlock.Dispose();
        }

        private void Initialize() {
            Line = GetComponentInParent<ToyLine>(true);
            if (Line == null) return;
            if (propertyBlock == null)
                propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetColor("_Color", Line.Color);
            var sphere = transform.Find("Sphere");
            if (sphere == null) return;
            var auraRenderer = sphere.GetComponent<MeshRenderer>();
            if (auraRenderer == null) return;
            auraRenderer.SetPropertyBlock(propertyBlock);
            auraRenderer.sortingOrder = 2;
        }

#if UNITY_EDITOR
        private void Update() {
            Initialize();
        }
#endif
    }
}
