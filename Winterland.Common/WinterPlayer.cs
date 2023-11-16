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
        public Player player = null;
        private SnowSinker snowSinker = null;
        private float snowTargetSize = 1f;
        private float snowLerpSpeed = 10f;

        public static WinterPlayer Get(Player player) {
            return player.GetComponent<WinterPlayer>();
        }

        private void Awake() {
            snowSinker = gameObject.AddComponent<SnowSinker>();
            snowSinker.Size = 1f;
            snowSinker.Strength = 1f;
        }

        private void Update() {
            snowSinker.Size = Mathf.Lerp(snowSinker.Size, snowTargetSize, snowLerpSpeed * Core.dt);
        }

        private void FixedUpdate() {
            snowSinker.Enabled = player.IsGrounded();
            if (!player.IsGrounded())
                snowTargetSize = 2f;
            else
                snowTargetSize = 1f;
            if (player.ability is SlideAbility && player.moveStyle != MoveStyle.BMX && player.moveStyle != MoveStyle.SKATEBOARD)
                snowTargetSize = 1.5f;

        }
    }
}
