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
        public const float MinimumGroundVertAngle = 20f;

        private void Awake() {
            ReptilePlayer = GetComponent<Player>();
        }

        public static MapStationPlayer Get(Player player) {
            var mpPlayer = player.GetComponent<MapStationPlayer>();
            if (mpPlayer == null)
                return player.gameObject.AddComponent<MapStationPlayer>();
            return mpPlayer;
        }
    }
}
