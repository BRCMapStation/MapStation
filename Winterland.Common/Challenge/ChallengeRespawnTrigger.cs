using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common.Challenge {
    public class ChallengeRespawnTrigger : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            var player = other.GetComponent<Player>();
            if (player == null)
                player = other.GetComponentInParent<Player>();
            if (player == null)
                return;
            if (player.isAI)
                return;
            StartCoroutine(RespawnRoutine(1f, 0.1f, 1f));
        }

        private IEnumerator RespawnRoutine(float fadeInDuration, float blackScreenDuration, float fadeOutDuration) {
            var effects = Core.Instance.UIManager.effects;
            effects.FadeInAndOutBlack(fadeInDuration, blackScreenDuration, fadeOutDuration);
            yield return new WaitForSeconds(fadeInDuration + blackScreenDuration);
            ChallengeLevel.CurrentChallengeLevel.Respawn();
        }
    }
}
