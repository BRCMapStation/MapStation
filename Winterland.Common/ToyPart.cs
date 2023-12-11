using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    public class ToyPart : MonoBehaviour {
        public ToyLine Line { get; private set; }
        public AudioClip PickUpAudioClip = null;

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
            Line = GetComponentInParent<ToyLine>(true);
        }
    }
}
