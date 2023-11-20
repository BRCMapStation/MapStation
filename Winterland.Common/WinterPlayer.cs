using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using UnityEngine;

namespace Winterland.Common {
    /// <summary>
    /// Holds custom data in players.
    /// </summary>
    public class WinterPlayer : MonoBehaviour {
        public ToyLine CurrentToyLine = null;
        [NonSerialized]
        public Player player = null;
        private SnowSinker snowSinker = null;
        private float snowTargetSize = 1f;
        private float snowTargetStrength = 1f;
        private readonly float snowLerpSpeed = 5f;

        public static WinterPlayer Get(Player player) {
            return player.GetComponent<WinterPlayer>();
        }

        private void Awake() {
            snowSinker = gameObject.AddComponent<SnowSinker>();
            snowSinker.Size = 1f;
            snowSinker.Strength = 0.5f;
        }

        private void Update() {
            snowSinker.Size = Mathf.Lerp(snowSinker.Size, snowTargetSize, snowLerpSpeed * Core.dt);
            snowSinker.Strength = Mathf.Lerp(snowSinker.Strength, snowTargetStrength, snowLerpSpeed * Core.dt);
        }

        private void FixedUpdate() {
            snowSinker.Enabled = player.IsGrounded();

            if (!player.IsGrounded()) {
                snowTargetSize = 2f;
                snowTargetStrength = 1f;
            } else {
                snowTargetSize = 1f;
                snowTargetStrength = 0.5f;
            }
            if (player.ability is SlideAbility && player.moveStyle != MoveStyle.BMX && player.moveStyle != MoveStyle.SKATEBOARD)
                snowTargetSize = 1.5f;
            if (player.moveStyle == MoveStyle.ON_FOOT && player.ability is not SlideAbility)
                snowTargetStrength = 0.2f;
            if (player.ability is GroundTrickAbility)
                snowTargetSize = 1.5f;
        }
    }
}
