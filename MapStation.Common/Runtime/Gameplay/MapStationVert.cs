using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MapStation.Common.Gameplay {
    public class MapStationVert : MonoBehaviour {
        public static GameObject DebugPrefab;
        public GameObject debugPrefab;
        private void Awake() {
            if (debugPrefab != null)
                DebugPrefab = debugPrefab;
        }

        public static void CreateDebugObject(Vector3 position, Quaternion rotation) {
            var debugObject = Instantiate(DebugPrefab);
            debugObject.transform.position = position;
            debugObject.transform.rotation = rotation;
        }
    }
}
