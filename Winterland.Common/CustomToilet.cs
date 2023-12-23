using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public class CustomToilet : MonoBehaviour {
        private static GameObject Source = null;

        private void Awake() {
            if (Source == null) {
                Source = FindObjectOfType<PublicToilet>().gameObject;
            }
            var myToilet = Instantiate(Source, transform);
            myToilet.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }
}
