using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MapStation.Common.Gameplay;
using Reptile;
using Microsoft.SqlServer.Server;

namespace MapStation.Plugin.Gameplay {
    public class MapStationPlayer : MonoBehaviour {
        public Player ReptilePlayer { get; private set; } = null;
        public Vector3 GroundVertVector = Vector3.down;

        public Collider VertCollider = null;
        public bool OnVertGround = false;
        public bool WasOnVertGround = false;
        public bool HasVertBelow = false;
        public const float MinimumGroundVertAngle = 10f;

        public bool OnVertAir = false;
        public const float MinimumAirVertAngle = 50f;
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

        public const float VertHorizontalSpeedMultiplier = 3f;

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
            ReptilePlayer = GetComponent<Player>();
            Core.OnUpdate += OnUpdate;
            Core.OnFixedUpdate += OnFixedUpdate;
        }

        public static MapStationPlayer Get(Player player) {
            var mpPlayer = player.GetComponent<MapStationPlayer>();
            if (mpPlayer == null) {
                Debug.LogWarning($"Requested MapStationPlayer component from player {player.gameObject.name} but they didn't have one.");
                return player.gameObject.AddComponent<MapStationPlayer>();
            }
            return mpPlayer;
        }

        private Quaternion currentVertRotation = Quaternion.identity;
        private float vertRotationSpeed = 5f;
        public void UpdateVertRotation() {
            var targetVertRotation = Quaternion.LookRotation(ReptilePlayer.motor.velocity.normalized, AirVertVector);
            currentVertRotation = Quaternion.Lerp(currentVertRotation, targetVertRotation, vertRotationSpeed * Core.dt);
            ReptilePlayer.SetRotation(Quaternion.LookRotation(currentVertRotation * Vector3.forward, Vector3.up));
            ReptilePlayer.SetVisualRot(currentVertRotation);
        }

        private void ResetVertRotation() {
            currentVertRotation = Quaternion.LookRotation(ReptilePlayer.visualTf.forward, AirVertVector);
        }

        public void AirVertBegin() {
            if (ReptilePlayer.ability is BoostAbility)
                VertBoostCooldown = VertBoostCooldownMax;
            // Carrying over abilities might screw with rotations
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

            // If we're landing back on vert calculate a rotation that goes down the slope.
            if (OnVertGround) {
                //var targetVector = (Vector3.down - Vector3.Project(Vector3.down, ReptilePlayer.motor.groundNormal)).normalized;
                var motorDir = ReptilePlayer.motor.velocity.normalized;
                var targetVector = (motorDir - Vector3.Project(motorDir, ReptilePlayer.motor.groundNormal)).normalized;
                var targetRotation = Quaternion.LookRotation(targetVector, ReptilePlayer.motor.groundNormal);
                ReptilePlayer.SetRotation(targetRotation);
            }

            if (ReptilePlayer.motor.isOnGround && ReptilePlayer.motor.isValidGround) {
                if (Vector3.Angle(ReptilePlayer.motor.groundNormal, Vector3.up) < MinimumAngleToKeepVertSpeed)
                    SpeedFromVertAir = 0f;
            } else
                SpeedFromVertAir = 0f;

            // Rotating the player too steep causes issues.
            ReptilePlayer.FlattenRotation();
        }

        public void AirVertUpdate() {
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
                if (Vector3.Angle(ReptilePlayer.motor.groundNormal, Vector3.up) < 10f)
                    ReptilePlayer.visualTf.transform.position = ReptilePlayer.motor.groundPoint;
                else
                    ReptilePlayer.characterVisual.feetIK = false;
            }
        }

        private void SnapToFloor() {
            var groundNormal = ReptilePlayer.motor.groundNormal;
            var groundPoint = ReptilePlayer.motor.groundPoint;

            var groundDelta = ReptilePlayer.transform.position - groundPoint;
            groundDelta -= Vector3.Project(groundDelta, groundNormal);
            groundDelta += 0.01f * groundNormal;

            ReptilePlayer.transform.position = groundDelta + groundPoint;
        }

        private void OnFixedUpdate() {
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

            if (OnVertGround && Vector3.Angle(ReptilePlayer.motor.groundNormal, Vector3.up) < MinimumGroundVertAngle)
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
