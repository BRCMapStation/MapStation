using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common.Challenge {
    public class ChallengeCheckpoint : MonoBehaviour {
        public Transform RespawnPoint = null;

        private void OnTriggerEnter(Collider other) {
            var player = other.GetComponent<Player>();
            if (player == null)
                player = other.GetComponentInParent<Player>();
            if (player == null)
                return;
            if (player.isAI)
                return;
            ChallengeLevel.CurrentChallengeLevel.SetRespawnPoint(RespawnPoint);
            ChallengeLevel.CurrentChallengeLevel.StartTimer();
        }
    }
}
