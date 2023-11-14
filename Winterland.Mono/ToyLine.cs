using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Winterland.Mono {
    public class ToyLine : MonoBehaviour {
        // We can use this to mark this line as complete between scene transitions and saving/loading.
        public string GUID;

        private void Reset() {
            GUID = Guid.NewGuid().ToString();
        }
    }
}
