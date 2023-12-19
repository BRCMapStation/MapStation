using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common.Challenge {
    public class ChallengeLevel : MonoBehaviour {
        public static ChallengeLevel CurrentChallengeLevel = null;
        public Transform SpawnPoint = null;
        public bool StartWithGear = false;
        private Transform currentRespawnPoint = null;
        private float timer = 0f;
        private bool timerStarted = false;

        public void StartChallenge() {
            var player = WorldHandler.instance.GetCurrentPlayer();
            player.phone.TurnOff();
            currentRespawnPoint = SpawnPoint;
            CurrentChallengeLevel = this;
            timer = 0f;
            timerStarted = false;
            WorldHandler.instance.PlacePlayerAt(player, currentRespawnPoint);
            player.SwitchToEquippedMovestyle(StartWithGear, false, true, false);
        }

        public void EndChallenge() {
            var player = WorldHandler.instance.GetCurrentPlayer();
        }

        public void Respawn() {
            var player = WorldHandler.instance.GetCurrentPlayer();
            WorldHandler.instance.PlacePlayerAt(player, currentRespawnPoint);
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
