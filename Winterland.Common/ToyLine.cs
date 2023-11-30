using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Winterland.Common {
    public class ToyLine : MonoBehaviour {
        // We can use this to mark this line as complete between scene transitions and saving/loading.
        public string GUID;
        public ToyPart[] ToyParts => toyParts;
        private ToyPart[] toyParts = null;

        public void Respawn() {
            var parts = ToyParts;
            foreach(var part in parts) {
                part.Respawn();
            }
        }

        private void Awake() {
            toyParts = GetComponentsInChildren<ToyPart>(true);
        }

        private void Reset() {
            GUID = Guid.NewGuid().ToString();
        }
    }
}
