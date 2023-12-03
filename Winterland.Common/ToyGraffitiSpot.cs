using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    [RequireComponent(typeof(GraffitiSpot))]
    public class ToyGraffitiSpot : MonoBehaviour {
        [HideInInspector]
        public ToyMachine ToyMachine = null;
        private void Awake() {
            ToyMachine = GetComponentInParent<ToyMachine>();
        }

        public static ToyGraffitiSpot Get(GraffitiSpot graffitiSpot) {
            return graffitiSpot.GetComponent<ToyGraffitiSpot>();
        }
    }
}
