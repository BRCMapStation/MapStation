using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common.Challenge {
    public class ChallengeLevel : MonoBehaviour {
        [Header("Sequences")]
        public CustomNPC ArcadeNPC = null;
        public Sequence FinishSequence = null;
        public Sequence FinishNewBestTimeSequence = null;
        public Sequence QuitSequence = null;
        [Header("General")]
        public static ChallengeLevel CurrentChallengeLevel = null;
        public Transform SpawnPoint = null;
        public bool StartWithGear = false;
        public bool TimerStarted => timerStarted;
        public float Timer => timer;
        public Action OnStart;
        public string GUID;
        [HideInInspector]
        public float BestTime;
        private Transform currentRespawnPoint = null;
        private float timer = 0f;
        private bool timerStarted = false;
        [HideInInspector]
        public ChallengeCheckpoint[] Checkpoints = null;

        // Temporary
        public Transform exitTransform = null;

        private void Awake() {
            Core.OnUpdate += Core_Update;
            Checkpoints = GetComponentsInChildren<ChallengeCheckpoint>(true);
            BestTime = WinterProgress.Instance.LocalProgress.GetChallengeBestTime(this);
        }

        private void Reset() {
            GUID = Guid.NewGuid().ToString();
        }

        private void OnDestroy() {
            Core.OnUpdate -= Core_Update;
        }

        public void StartChallenge() {
            WinterUI.Instance.ChallengeUI.Visible = true;
            var player = WorldHandler.instance.GetCurrentPlayer();
            player.phone.TurnOff();
            player.boostCharge = player.maxBoostCharge;
            currentRespawnPoint = SpawnPoint;
            CurrentChallengeLevel = this;
            timer = 0f;
            timerStarted = false;
            WorldHandler.instance.PlacePlayerAt(player, currentRespawnPoint);
            player.SwitchToEquippedMovestyle(StartWithGear, false, true, false);
            OnStart?.Invoke();
        }

        public void FinishChallenge() {
            var player = WorldHandler.instance.GetCurrentPlayer();
            player.boostCharge = 0f;
            WinterUI.Instance.ChallengeUI.Visible = false;
            timerStarted = false;
            CurrentChallengeLevel = null;
            if (BestTime == 0f || Timer < BestTime) {
                var progress = WinterProgress.Instance.LocalProgress;
                BestTime = Timer;
                progress.SetChallengeBestTime(this, BestTime);
                progress.Save();
                ArcadeNPC.StartSequence(FinishNewBestTimeSequence);
            } else {
                ArcadeNPC.StartSequence(FinishSequence);
            }
        }

        public void QuitChallenge() {
            var player = WorldHandler.instance.GetCurrentPlayer();
            player.boostCharge = 0f;
            WinterUI.Instance.ChallengeUI.Visible = false;
            CurrentChallengeLevel = null;
            ArcadeNPC.StartSequence(QuitSequence);
        }

        public void Respawn() {
            if (currentRespawnPoint == SpawnPoint) {
                StartChallenge();
            } else {
                var player = WorldHandler.instance.GetCurrentPlayer();
                WorldHandler.instance.PlacePlayerAt(player, currentRespawnPoint);
            }
        }
        
        public void StartTimer() {
            if (timerStarted)
                return;
            timerStarted = true;
            timer = 0f;
        }

        public void SetRespawnPoint(Transform respawnPoint) {
            currentRespawnPoint = respawnPoint;
        }

        private void Core_Update() {
            if (timerStarted)
                timer += Core.dt;
        }
    }
}
