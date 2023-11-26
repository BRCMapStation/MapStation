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
        public bool OnSnow => OffSnow <= 0;
        public int OffSnow = 0;
        public ToyLine CurrentToyLine = null;
        [NonSerialized]
        public Player player = null;
        private SnowSinker snowSinker = null;
        private float snowTargetSize = 1f;
        private float snowTargetStrength = 1f;
        private readonly float snowLerpSpeed = 5f;
        private ParticleSystem snowParticles = null;

        public static WinterPlayer Get(Player player) {
            return player.GetComponent<WinterPlayer>();
        }

        private void Awake() {
            if (FallenSnowController.Instance != null) {
                if (FallenSnowController.Instance.SnowFootstepParticlesPrefab != null) {
                    var snowInstance = GameObject.Instantiate(FallenSnowController.Instance.SnowFootstepParticlesPrefab);
                    snowInstance.transform.SetParent(transform);
                    snowInstance.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                    snowParticles = snowInstance.GetComponentInChildren<ParticleSystem>();
                    var emission = snowParticles.emission;
                    emission.enabled = false;
                }
            }
            snowSinker = gameObject.AddComponent<SnowSinker>();
            snowSinker.Size = 1f;
            snowSinker.Strength = 0.5f;
        }

        private void Update() {
            snowSinker.Size = Mathf.Lerp(snowSinker.Size, snowTargetSize, snowLerpSpeed * Core.dt);
            snowSinker.Strength = Mathf.Lerp(snowSinker.Strength, snowTargetStrength, snowLerpSpeed * Core.dt);
        }

        private void OnTriggerStay(Collider other) {
            if (other.gameObject.layer != 19)
                return;
            if (other.gameObject.name != "Snowless Ground Volume")
                return;
            OffSnow = 1;
        }

        public bool IsOnLevelGround() {
            if (player.IsOnNonStableGround())
                return false;
            if (player.motor.groundRigidbody != null)
                return false;
            if (player.motor.isOnPlatform)
                return false;
            if (player.IsGrounded())
                return true;
            return false;
        }

        private void FixedUpdate() {
            var snowFX = IsOnLevelGround() && OnSnow;
            snowSinker.Enabled = snowFX;

            if (snowParticles != null) {
                var emission = snowParticles.emission;
                if (snowFX && player.GetVelocity().sqrMagnitude >= FallenSnowController.Instance.MinimumSpeedForSnowParticles) {
                    emission.enabled = true;
                } else
                    emission.enabled = false;
            }

            if (!player.IsGrounded()) {
                snowTargetSize = 2f;
                snowTargetStrength = 1f;
            } else {
                snowTargetSize = 1f;
                snowTargetStrength = 0.5f;
            }
            /*
            if (player.ability is SlideAbility && player.moveStyle != MoveStyle.BMX && player.moveStyle != MoveStyle.SKATEBOARD)
                snowTargetSize = 1.5f;*/
            if (player.moveStyle == MoveStyle.ON_FOOT && player.ability is not SlideAbility)
                snowTargetStrength = 0.2f;
            if (player.ability is GroundTrickAbility)
                snowTargetSize = 1.5f;
            OffSnow -= 1;
            if (OffSnow <= 0)
                OffSnow = 0;
        }
    }
}
