using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Winterland.Mono {
    public class PickupRotation : MonoBehaviour {
        [SerializeField]
        private Vector3 rotationSpeed;

        private void Update() {
            transform.Rotate(rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}
