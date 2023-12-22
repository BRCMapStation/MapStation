using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using UnityEngine;

namespace Winterland.Common {
    /// <summary>
    /// Holds custom data in players.
    /// </summary>
    public class WinterPlayer : MonoBehaviour {
        public bool Local {
            get {
                return player == WorldHandler.instance.GetCurrentPlayer();
            }
        }
        public bool SnowFX = true;
        public bool SnowDeform = true;
        public ToyLine CurrentToyLine = null;
        public int CollectedToyParts = 0;
        public ToyMachineAbility ToyMachineAbility = null;
        // If we exceed the max amount of tree trampoline bounces in a row we break our combo. To stop infinite scoring.
        public int TimesTrampolined = 0;
        public const int MaxTimesTrampolined = 1;
        [NonSerialized]
        public Player player = null;
        public CameraZoomZone CurrentCameraZoomZone = null;
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
            Core.OnFixedUpdate += OnFixedUpdate;
        }

        private void OnDestroy() {
            Core.OnFixedUpdate -= OnFixedUpdate;
        }

        private void Update() {
            snowSinker.Size = Mathf.Lerp(snowSinker.Size, snowTargetSize, snowLerpSpeed * Core.dt);
            snowSinker.Strength = Mathf.Lerp(snowSinker.Strength, snowTargetStrength, snowLerpSpeed * Core.dt);
        }

        // TODO: sound effects and stuff for toy part picking up.
        // This is a lot faster to process per player than per individual item.
        private void ProcessPickupTriggers(Collider other) {
            if (player.currentComboMascotSystem != null)
                return;
            if (!Local)
                return;
            var toyPart = other.GetComponent<ToyPart>();
            if (toyPart == null)
                return;
            if (player.IsDead())
                return;
            if (player.IsBusyWithSequence())
                return;
            if (!player.IsComboing())
                return;
            if (CurrentToyLine != null) {
                if (CurrentToyLine != toyPart.Line)
                    return;
            }
            toyPart.Collect(player);
        }

        private void ProcessToyMachineTriggers(Collider other) {
            if (other.gameObject.layer != 19)
                return;
            var toyMachineFreeze = other.GetComponent<ToyMachineFreeze>();
            if (toyMachineFreeze == null)
                return;
            if (player.isAI)
                return;
            toyMachineFreeze.Activate(player);
        }

        private void ProcessSnowTriggers(Collider other) {
            if (other.gameObject.layer != 19)
                return;
            if (other.gameObject.name.StartsWith("Snowless Ground Volume")) {
                SnowFX = false;
                SnowDeform = false;
                return;
            }
            if (other.gameObject.name.StartsWith("No Snow Deform Ground Volume")) {
                SnowFX = true;
                SnowDeform = false;
                return;
            }
            if (other.gameObject.name.StartsWith("No Snow FX Ground Volume")) {
                SnowFX = false;
                SnowDeform = true;
                return;
            }
        }

        private void OnTriggerStay(Collider other) {
            ProcessPickupTriggers(other);
            ProcessSnowTriggers(other);
            ProcessToyMachineTriggers(other);
        }

        

        public bool IsOnLevelGround() {
            if (player.motor.groundCollider != null) {
                if (player.motor.groundCollider.gameObject.GetComponentInParent<SnowlessSurface>() != null || player.motor.groundCollider.gameObject.GetComponent<SnowlessSurface>() != null)
                    return false;
            }
            if (player.ability is WallrunLineAbility || player.ability is GrindAbility || player.ability is HandplantAbility)
                return false;
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

        public void FinishCurrentToyLine() {
            if (CurrentToyLine == null)
                return;
            CurrentToyLine.Collect();
            CurrentToyLine = null;
            if (WinterUI.Instance != null && Local) {
                var toyLineUI = WinterUI.Instance.ToyLineUI;
                toyLineUI.Visible = false;
            }
        }

        public void DropCurrentToyLine() {
            if (CurrentToyLine == null)
                return;
            CurrentToyLine.Respawn();
            CurrentToyLine = null;
            if (WinterUI.Instance != null && Local) {
                var toyLineUI = WinterUI.Instance.ToyLineUI;
                toyLineUI.Visible = false;
            }
        }

        public static WinterPlayer GetLocal() {
            var localPlayer = WorldHandler.instance.GetCurrentPlayer();
            if (localPlayer == null)
                return null;
            return Get(localPlayer);
        }

        private void OnFixedUpdate() {

            if (!player.IsComboing()) {
                if (CurrentToyLine != null)
                    DropCurrentToyLine();
            }

            if (player.IsGrounded() || player.ability is WallrunLineAbility || player.ability is GrindAbility)
                TimesTrampolined = 0;

            if (WinterUI.Instance != null && Local) {
                var toyLineUI = WinterUI.Instance.ToyLineUI;
                toyLineUI.Visible = CurrentToyLine != null;
            }

            var snowFX = IsOnLevelGround() && SnowFX;
            var snowDeform = IsOnLevelGround() && SnowDeform;
            snowSinker.Enabled = snowDeform;

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
                snowTargetStrength = 1.0f;
            }
            /*
            if (player.ability is SlideAbility && player.moveStyle != MoveStyle.BMX && player.moveStyle != MoveStyle.SKATEBOARD)
                snowTargetSize = 1.5f;*/
            if (player.moveStyle == MoveStyle.ON_FOOT && player.ability is not SlideAbility)
                snowTargetStrength = 0.4f;

            if (player.ability is SlideAbility)
                snowTargetStrength = 1f;

            if (player.ability is GroundTrickAbility)
                snowTargetSize = 1.5f;
            SnowDeform = true;
            SnowFX = true;
        }
    }
}
