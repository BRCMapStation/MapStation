using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MapStation.Common.Runtime.Gameplay {
    public class MapStationPublicToilet : MonoBehaviour {
        private void Awake() {
#if BEPINEX
            var myToilet = StagePrefabHijacker.Prefabs.GetToilet();
            myToilet.transform.SetParent(transform, false);
            myToilet.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
#endif
        }
    }
}
