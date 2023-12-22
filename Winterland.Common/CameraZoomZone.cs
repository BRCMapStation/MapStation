using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    public class CameraZoomZone : MonoBehaviour {
        public bool ChangeCameraPosition = false;
        public float CameraDragDistanceDefault = 1f;
        public float CameraDragDistanceMax = 1f;
        public float CameraHeight = 2f;
        public bool ChangeCameraFOV = false;
        public float CameraFOV = 40f;

        private void OnTriggerEnter(Collider other) {
            var player = other.GetComponent<Player>();
            if (player == null)
                player = other.GetComponentInParent<Player>();
            if (player == null) return;
            if (player.isAI) return;
            var winterPlayer = WinterPlayer.Get(player);
            if (winterPlayer == null) return;
            winterPlayer.CurrentCameraZoomZone = this;
        }

        private void OnTriggerExit(Collider other) {
            var player = other.GetComponent<Player>();
            if (player == null)
                player = other.GetComponentInParent<Player>();
            if (player == null) return;
            if (player.isAI) return;
            var winterPlayer = WinterPlayer.Get(player);
            if (winterPlayer == null) return;
            if (winterPlayer.CurrentCameraZoomZone == this)
                winterPlayer.CurrentCameraZoomZone = null;
        }
    }
}
