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
        public MapStationVert CurrentVert = null;

        private void Awake() {
            ReptilePlayer = GetComponent<Player>();
        }
    }
}
