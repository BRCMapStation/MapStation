using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Winterland.Common {
    public class ToyLine : MonoBehaviour {
        // We can use this to mark this line as complete between scene transitions and saving/loading.
        public string GUID;
        public Guid Guid => Guid.Parse(GUID);
        public ToyPart[] ToyParts => toyParts;
        private ToyPart[] toyParts = null;

        public void Respawn() {
            var localProgress = WinterProgress.Instance.LocalProgress;
            localProgress.SetToyLineCollected(Guid, false);
            localProgress.Save();
            foreach (var part in toyParts) {
                part.Respawn();
            }
        }

        public void Collect() {
            var localProgress = WinterProgress.Instance.LocalProgress;
            localProgress.SetToyLineCollected(Guid, true);
            localProgress.Save();
            HideParts();
        }

        private void HideParts() {
            foreach (var part in toyParts) {
                part.gameObject.SetActive(false);
            }
        }

        private void Awake() {
            toyParts = GetComponentsInChildren<ToyPart>(true);
            var localProgress = WinterProgress.Instance.LocalProgress;
            if (localProgress.IsToyLineCollected(Guid))
                HideParts();
            ToyLineManager.Instance.RegisterToyLine(this);
        }

        private void Reset() {
            GUID = Guid.NewGuid().ToString();
        }
    }
}
