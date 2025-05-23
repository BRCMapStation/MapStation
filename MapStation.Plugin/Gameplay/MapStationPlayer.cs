using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MapStation.Common.Gameplay;
using Reptile;
using Microsoft.SqlServer.Server;
using ECM.Common;
using MapStation.Common;

namespace MapStation.Plugin.Gameplay {
    public class MapStationPlayer : MonoBehaviour {
        public float GroundCooldown = 0f;
        public Player ReptilePlayer { get; private set; } = null;
        public Vector3 GroundVertVector = Vector3.down;

        public Collider VertCollider = null;
        public bool OnVertGround = false;
        public bool WasOnVertGround = false;
        public bool HasVertBelow = false;
        public const float MinimumGroundVertAngle = 10f;

        public bool OnVertAir = false;
        public const float MinimumAirVertAngle = 45f;
        public Vector3 AirVertVector = Vector3.down;
        public float SpeedFromVertAir = 0f;

        public float VertBoostCooldown = 0f;

        public const float VertLandMultiplier = 0.6f;
        public const float VertLandDecc = 4f;
        public const float MinimumVertGravityAngle = 45f;
        public const float VertMinimumSpeed = 2f;
        public const float VertGravity = 20f;
        public const float VertGravityTurnSpeed = 4f;
        public const float MinimumAngleToKeepVertSpeed = 10f;
        public const float VertBoostCooldownMax = 0.5f;

        public const float VertOuterNudge = 0.35f;
        public const float VertOuterRay = 0.35f;
        public const float VertInnerNudge = 0.35f;
        public const float VertInnerRay = 0.35f;

        public const float VertHorizontalSpeedMultiplier = 1.5f;
        [HideInInspector]
        public CameraMode VertCameraMode = null;

        public MapStationHandplantAbility MapStationHandplantAbility { get; private set; }

        public bool MoveStyleEquipped {
            get {
                if (ReptilePlayer.usingEquippedMovestyle) return true;
                if (ReptilePlayer.moveStyleEquipped == MoveStyle.INLINE) return false;
                var boostAbility = ReptilePlayer.ability as BoostAbility;
                if (boostAbility == null) return false;
                if (boostAbility.equippedMovestyleWasUsed) return true;
                return false;
            }
        }

        private void Awake() {
            VertCameraMode = new CameraModeVert(null);
            ReptilePlayer = GetComponent<Player>();
            Core.OnUpdate += OnUpdate;
            Core.OnFixedUpdate += OnFixedUpdate;
        }

        public bool HoldingVertGrindButton() {
            return ReptilePlayer.trick1ButtonHeld || ReptilePlayer.trick1ButtonNew;
        }

        /// <summary>
        /// Called immediately after Reptile.Player.Init
        /// </summary>
        public void Init() {
            MapStationHandplantAbility = new MapStationHandplantAbility(ReptilePlayer.handplantAbility);
        }

        // HACK - For whatever reason, lazy loading fixes a bug where teleporting while on a vanilla vert would send you to 0,0,0 and maybe some other things?
        // However lazy loading causes this component to be duplicated, so :/ keeping this behavior to fix the above regression as it doesn't seem to cause issues otherwise.
        // Originally this was reworked in 0.15.0 to fix the duplicate component issue but i'm bringing it back in 01/05/25 to fix the teleport issue. Should look into this further.
        public static MapStationPlayer Get(Player player) {
            var mpPlayer = player.GetComponent<MapStationPlayer>();
            if (mpPlayer == null) {
                //Log.Warning($"Requested MapStationPlayer component from player {player.gameObject.name} but they didn't have one.");
                return player.gameObject.AddComponent<MapStationPlayer>();
            }
            return mpPlayer;
        }

        private Quaternion currentVertRotation = Quaternion.identity;
        private float vertRotationSpeed = 5f;
        public void UpdateVertRotation() {
            var targetVertRotation = Quaternion.LookRotation(ReptilePlayer.motor.velocity.normalized, AirVertVector);
            currentVertRotation = Quaternion.Lerp(currentVertRotation, targetVertRotation, vertRotationSpeed * Core.dt);
            ReptilePlayer.SetVisualRot(currentVertRotation);
        }

        private void ResetVertRotation() {
            currentVertRotation = Quaternion.LookRotation(ReptilePlayer.visualTf.forward, AirVertVector);
        }

        public void AirVertBegin() {
            if (ReptilePlayer.ability is BoostAbility)
                VertBoostCooldown = VertBoostCooldownMax;
            // Carrying over abilities might screw with things
            ReptilePlayer.StopCurrentAbility();
            OnVertAir = true;
            AirVertVector = GroundVertVectorToAir(GroundVertVector);
            var horizontalSpeed = Vector3.Project(ReptilePlayer.motor.velocity, Vector3.Cross(AirVertVector, Vector3.up));
            ReptilePlayer.motor.velocity -= horizontalSpeed;
            ReptilePlayer.motor.velocity += horizontalSpeed * VertHorizontalSpeedMultiplier;
            RemoveAirVertSpeed();
            ResetVertRotation();
            if (VertCollider != null)
                ReptilePlayer.UseColliderInCombo(VertCollider);
            ReptilePlayer.PlayAnim(Animator.StringToHash("jump"), false, false, -1f);
            SpeedFromVertAir = 0f;
            // lil nudge so we don't get caught up in shit
            //ReptilePlayer.motor.transform.position += AirVertVector * 0.1f;
        }

        public void AirVertEnd() {
            VertBoostCooldown = 0f;
            OnVertAir = false;

            if (ReptilePlayer.motor.isOnGround && ReptilePlayer.motor.isValidGround) {
                var targetRotation = Quaternion.FromToRotation(currentVertRotation * Vector3.up, Vector3.up) * currentVertRotation;
                ReptilePlayer.SetRotation(targetRotation);
                ReptilePlayer.motor.velocity = ReptilePlayer.motor.velocity.magnitude * (targetRotation * Vector3.forward);
                if (Vector3.Angle(ReptilePlayer.motor.groundNormal, Vector3.up) < MinimumAngleToKeepVertSpeed)
                    SpeedFromVertAir = 0f;
            } else
                SpeedFromVertAir = 0f;

            // Rotating the player too steep causes issues.
            ReptilePlayer.FlattenRotation();
        }

        public void AirVertUpdate() {
            // Hacky stuff - set our rotation either looking horizontally relative to the vert for movement and physics stuff to work better.
            var vertRight = Vector3.Cross(AirVertVector, Vector3.up).normalized;
            var vertForward = vertRight;
            var vertDot = Vector3.Dot(ReptilePlayer.motor.velocity.normalized, vertForward);
            if (vertDot < 0f)
                vertForward = -vertRight;
            ReptilePlayer.SetRotation(Quaternion.LookRotation(vertForward, Vector3.up));

            VertBoostCooldown -= Core.dt;
            if (VertBoostCooldown < 0f)
                VertBoostCooldown = 0f;
            // Accumulate speed to add when we land.
            var ySpeed = Mathf.Abs(ReptilePlayer.motor.velocity.y) * VertLandMultiplier;
            if (ySpeed > SpeedFromVertAir)
                SpeedFromVertAir = ySpeed;
            RemoveAirVertSpeed();
        }

        public void TransferVert(Vector3 newVertVector) {
            var vertRight = Vector3.Cross(AirVertVector, Vector3.up);
            var horizontalSpeed = Vector3.Dot(ReptilePlayer.motor.velocity, vertRight);
            ReptilePlayer.motor.velocity -= horizontalSpeed * vertRight;
            AirVertVector = newVertVector;
            ReptilePlayer.motor.velocity += horizontalSpeed * vertRight;
            RemoveAirVertSpeed();
        }

        // Convert the vert slope normal to air vert normal.
        private Vector3 GroundVertVectorToAir(Vector3 groundVector) {
            return GetVertVectorFromGroundNormal(-groundVector);
        }

        public static Vector3 GetVertVectorFromGroundNormal(Vector3 groundNormal) {
            groundNormal.y = 0f;
            return groundNormal.normalized;
        }

        private void RemoveAirVertSpeed() {
            ReptilePlayer.motor.velocity -= Vector3.Project(ReptilePlayer.motor.velocity, AirVertVector);
        }

        private void OnUpdate() {
            // IK can be very wonky on steep slopes.
            if (OnVertGround) {
                if (Vector3.Angle(ReptilePlayer.motor.groundNormal, Vector3.up) >= 10f)
                    ReptilePlayer.characterVisual.feetIK = false;
            }
        }

        private void SnapToFloor() {
            var groundNormal = ReptilePlayer.motor.groundNormal;
            var groundPoint = ReptilePlayer.motor.groundPoint;
            var maxDelta = 0.05f;

            var groundDelta = ReptilePlayer.transform.position - groundPoint;
            var currentDelta = Vector3.Dot(groundDelta, groundNormal);
            if (currentDelta > maxDelta) {
                groundDelta -= Vector3.Project(groundDelta, groundNormal);
                groundDelta += maxDelta * groundNormal;

                ReptilePlayer.transform.position = groundDelta + groundPoint;
            }
        }

        public bool OnPreventComboBreakingGrindPath() {
            if (ReptilePlayer.ability != ReptilePlayer.grindAbility)
                return false;
            var grindAbility = ReptilePlayer.grindAbility;
            return grindAbility.lastPath.GetComponent<GrindPath_FixComboBreakingProperty>() != null;
        }

        private void OnFixedUpdate() {

            if (!Core.Instance.IsCorePaused) {
                GroundCooldown = Mathf.Max(0f, GroundCooldown - Core.dt);
            }

            if (ReptilePlayer.IsGrounded()) {
                var targetVector = (ReptilePlayer.motor.dir - Vector3.Project(ReptilePlayer.motor.dir, ReptilePlayer.motor.groundNormal)).normalized;
                ReptilePlayer.motor.velocity += targetVector * SpeedFromVertAir * Core.dt;

                if (SpeedFromVertAir > 5f) {
                    if (ReptilePlayer.targetMovement == Player.MovementType.NONE)
                        ReptilePlayer.targetMovement = Player.MovementType.WALKING;
                }

                if (SpeedFromVertAir > 0f)
                    SpeedFromVertAir -= VertLandDecc * Core.dt;

                if (SpeedFromVertAir < 0f)
                    SpeedFromVertAir = 0f;
            } else
                SpeedFromVertAir = 0f;

            if (!MoveStyleEquipped)
                SpeedFromVertAir = 0f;

            if (ReptilePlayer.vertShape != null) return;
            if (ReptilePlayer.ability is VertAbility) return;

            if (OnVertGround && Vector3.Angle(ReptilePlayer.motor.groundNormal, Vector3.up) < MinimumVertGravityAngle) {
                ReptilePlayer.FlattenRotation();
            }

            if (OnVertGround && Vector3.Angle(ReptilePlayer.motor.groundNormal, Vector3.up) >= MinimumVertGravityAngle && ReptilePlayer.motor.velocity.magnitude < VertMinimumSpeed) {
                var normal = ReptilePlayer.motor.groundNormal;
                var targetVector = (Vector3.down - Vector3.Project(Vector3.down, normal)).normalized;
                var targetRotation = Quaternion.LookRotation(targetVector, normal);
                var currentRotation = Quaternion.Lerp(ReptilePlayer.motor.rotation, targetRotation, VertGravityTurnSpeed * Core.dt);
                ReptilePlayer.SetRotation(currentRotation);
                var currentForward = currentRotation * Vector3.forward;
                var downDot = Mathf.Max(0f, Vector3.Dot(currentForward, targetVector));
                var downAcceleration = downDot * VertGravity;
                ReptilePlayer.motor.velocity += currentForward * (downAcceleration * Core.dt);
                ReptilePlayer.targetMovement = Player.MovementType.WALKING;
            }

            if (OnVertGround && Vector3.Angle(ReptilePlayer.motor.groundNormal, Vector3.up) < MinimumVertGravityAngle)
                SnapToFloor();

            if (OnVertAir && !ReptilePlayer.IsGrounded())
                AirVertUpdate();
        }

        private void OnDestroy() {
            Core.OnUpdate -= OnUpdate;
            Core.OnFixedUpdate -= OnFixedUpdate;
        }
    }
}
