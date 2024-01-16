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
        public const float MinimumGroundVertAngle = 10f;

        public bool OnVertAir = false;
        public const float MinimumAirVertAngle = 30f;
        public Vector3 AirVertVector = Vector3.down;
        public float SpeedFromVertAir = 0f;

        public const float VertLandMultiplier = 1.25f;
        public const float VertLandDecc = 4f;
        public const float MinimumVertGravityAngle = 45f;
        public const float VertMinimumSpeed = 2f;
        public const float VertGravity = 20f;
        public const float VertGravityTurnSpeed = 4f;

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
            if (mpPlayer == null)
                return player.gameObject.AddComponent<MapStationPlayer>();
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
                ReptilePlayer.StopCurrentAbility();
            OnVertAir = true;
            AirVertVector = GroundVertVectorToAir(GroundVertVector);
            RemoveAirVertSpeed();
            ResetVertRotation();
            if (VertCollider != null)
                ReptilePlayer.UseColliderInCombo(VertCollider);
            ReptilePlayer.PlayAnim(Animator.StringToHash("jump"), false, false, -1f);
            SpeedFromVertAir = 0f;
        }

        public void AirVertEnd() {
            OnVertAir = false;
            if (OnVertGround) {
                var targetVector = (Vector3.down - Vector3.Project(Vector3.down, ReptilePlayer.motor.groundNormal)).normalized;
                var targetRotation = Quaternion.LookRotation(targetVector, ReptilePlayer.motor.groundNormal);
                ReptilePlayer.SetRotation(targetRotation);
                //ReptilePlayer.motor.velocity = targetVector * SpeedFromVertAir;
                //ReptilePlayer.SetRotHard(currentVertRotation);
            }
            ReptilePlayer.FlattenRotation();
            //ReptilePlayer.FlattenRotation();
        }

        public void AirVertUpdate() {
            var downSpeed = Mathf.Abs(ReptilePlayer.motor.velocity.y) * VertLandMultiplier;
            if (downSpeed > SpeedFromVertAir)
                SpeedFromVertAir = downSpeed;
            RemoveAirVertSpeed();
        }

        private Vector3 GroundVertVectorToAir(Vector3 groundVector) {
            var airVector = -groundVector;
            airVector.y = 0f;
            return airVector.normalized;
        }

        private void RemoveAirVertSpeed() {
            ReptilePlayer.motor.velocity -= Vector3.Project(ReptilePlayer.motor.velocity, AirVertVector);
        }

        private void OnUpdate() {
            if (OnVertGround) {
                if (Vector3.Angle(ReptilePlayer.motor.groundNormal, Vector3.up) < 10f)
                    ReptilePlayer.visualTf.transform.position = ReptilePlayer.motor.groundPoint;
                else
                    ReptilePlayer.characterVisual.feetIK = false;
            }
            //if (OnVertAir)
                //UpdateVertRotation();
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

            /*
            if (OnVertGround) {
                var heightOffVertGround = 0.1f;
                var positionDelta = ReptilePlayer.motor.transform.position - ReptilePlayer.motor.groundPoint;
                positionDelta -= Vector3.Project(positionDelta, ReptilePlayer.motor.groundNormal);
                positionDelta += heightOffVertGround * ReptilePlayer.motor.groundNormal;
                ReptilePlayer.motor.transform.position = positionDelta + ReptilePlayer.motor.groundPoint;
            }*/

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

            if (OnVertAir && !ReptilePlayer.IsGrounded())
                AirVertUpdate();
        }

        private void OnDestroy() {
            Core.OnUpdate -= OnUpdate;
            Core.OnFixedUpdate -= OnFixedUpdate;
        }
    }
}
