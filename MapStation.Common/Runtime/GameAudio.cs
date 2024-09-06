using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MapStation.Common.Runtime {
    [RequireComponent(typeof(AudioSource))]
    public class GameAudio : MonoBehaviour {
        public enum AudioTypes {
            Master = 0,
            Music = 4,
            SFX = 1,
            UI = 2,
            Gameplay = 3,
            Voices = 5,
            Ambience = 6
        }
        public AudioTypes AudioType = AudioTypes.Gameplay;
#if BEPINEX
        private void Awake() {
            var audioSource = GetComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = Core.Instance.AudioManager.mixerGroups[(int) AudioType];
        }
#endif
    }
}
