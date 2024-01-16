using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MapStation.Common.Gameplay;
using Reptile;

namespace MapStation.Plugin.Gameplay {
    public class MapStationPlayer : MonoBehaviour {
        public Player ReptilePlayer { get; private set; } = null;
        public Vector3 GroundVertVector = Vector3.down;

        public bool OnVertGround = false;
        public bool WasOnVertGround = false;
        public const float MinimumGroundVertAngle = 10f;

        public bool OnVertAir = false;
        public const float MinimumAirVertAngle = 30f;
        public Vector3 AirVertVector = Vector3.down;

        private void Awake() {
            ReptilePlayer = GetComponent<Player>();
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
            ReptilePlayer.SetRotHard(currentVertRotation);
            ReptilePlayer.SetVisualRot(currentVertRotation);
        }

        private void ResetVertRotation() {
            currentVertRotation = Quaternion.LookRotation(ReptilePlayer.visualTf.forward, AirVertVector);
        }

        public void AirVertBegin() {
            OnVertAir = true;
            AirVertVector = GroundVertVectorToAir(GroundVertVector);
            RemoveAirVertSpeed();
            ResetVertRotation();
        }

        public void AirVertEnd() {
            OnVertAir = false;
            if (OnVertGround) {
                ReptilePlayer.SetRotHard(currentVertRotation);
            }
        }

        public void AirVertUpdate() {
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

        private void Update() {
            //if (OnVertAir)
                //UpdateVertRotation();
        }

        private void FixedUpdate() {
            if (OnVertAir && !ReptilePlayer.IsGrounded())
                AirVertUpdate();
        }
    }
}
