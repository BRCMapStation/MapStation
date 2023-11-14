using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Winterland.Common {
    public class ToyLine : MonoBehaviour {
        // We can use this to mark this line as complete between scene transitions and saving/loading.
        public string GUID;
        public ToyPart[] ToyParts => GetComponentsInChildren<ToyPart>(true);

        public void Respawn() {
            var parts = ToyParts;
            foreach(var part in parts) {
                part.Respawn();
            }
        }

        private void Reset() {
            GUID = Guid.NewGuid().ToString();
        }
    }
}
