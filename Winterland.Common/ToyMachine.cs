using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    public class ToyMachine : MonoBehaviour {
        public float TimeInSecondsToSpray = 1f;
        public float PlayerExitSpeed = 20f;
        public float PlayerExitUpwardsSpeed = 0f;
        public Transform PlayerExitLocation = null;
        public float FadeInDuration = 0.2f;
        public float FadeOutDuration = 0.2f;
        public float BlackScreenDuration = 0.1f;
        public AudioClip EnterToyMachineAudioClip = null;
        public AudioClip SuccessAudioClip = null;
        public AudioClip FailureAudioClip = null;

        public void FinishToyLine() {
            var player = WorldHandler.instance.GetCurrentPlayer();
            var winterPlayer = WinterPlayer.Get(player);
            if (winterPlayer == null)
                return;
            if (winterPlayer.CurrentToyLine == null) {
                PlayFailureSound();
                return;
            }
            if (winterPlayer.CollectedToyParts != winterPlayer.CurrentToyLine.ToyParts.Length) {
                PlayFailureSound();
                winterPlayer.DropCurrentToyLine();
            } else {
                PlaySuccessSound();
                winterPlayer.FinishCurrentToyLine();
            }
        }

        private void PlaySound(AudioClip clip) {
            if (clip == null)
                return;
            var audioManager = Core.Instance.AudioManager;
            audioManager.PlayNonloopingSfx(audioManager.audioSources[3], clip, audioManager.mixerGroups[3], 0f);
        }

        public void PlayEnterToyMachineSound() {
            PlaySound(EnterToyMachineAudioClip);
        }

        public void PlaySuccessSound() {
            PlaySound(SuccessAudioClip);
        }

        public void PlayFailureSound() {
            PlaySound(FailureAudioClip);
        }

        public void TeleportPlayerToExit(bool graffiti) {
            var player = WorldHandler.instance.GetCurrentPlayer();
            WorldHandler.instance.PlaceCurrentPlayerAt(PlayerExitLocation);
            var velocity = PlayerExitLocation.forward * PlayerExitSpeed;
            velocity += Vector3.up * PlayerExitUpwardsSpeed;
            player.SetVelocity(velocity);
            if (!graffiti) {
                var effects = Core.Instance.UIManager.effects;
                effects.FullBlackToFadeOut(0f, FadeInDuration);
            }
        }
    }
}
