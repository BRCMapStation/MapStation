using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common.Challenge {
    public class ChallengeCheckpoint : MonoBehaviour {
        public bool IsFinish = false;
        public ChallengeLevel Level => owner;
        public Transform RespawnPoint = null;
        [HideInInspector]
        public bool Hit = false;
        private ChallengeLevel owner = null;

        private void Awake() {
            owner = GetComponentInParent<ChallengeLevel>();
            owner.OnStart += OnStartChallenge;
        }

        private void OnStartChallenge() {
            Hit = false;
        }

        private void OnTriggerEnter(Collider other) {
            var player = other.GetComponent<Player>();
            if (player == null)
                player = other.GetComponentInParent<Player>();
            if (player == null)
                return;
            if (player.isAI)
                return;
            var checkpointSFX = WinterAssets.Instance.CheckpointClip;
            if (!IsFinish) {
                if (checkpointSFX != null && !Hit) {
                    var audioManager = Core.Instance.AudioManager;
                    audioManager.PlayNonloopingSfx(audioManager.audioSources[3], checkpointSFX, audioManager.mixerGroups[3], 0f);
                }
                owner.SetRespawnPoint(RespawnPoint);
                owner.StartTimer();
                Hit = true;
            } else {
                foreach (var checkpoint in owner.Checkpoints) {
                    if (checkpoint.IsFinish)
                        continue;
                    if (!checkpoint.Hit)
                        return;
                }
                if (!owner.TimerStarted)
                    return;
                owner.FinishChallenge();
            }
        }
    }
}
