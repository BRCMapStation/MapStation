using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public class ToyLineManager : MonoBehaviour {
        public static ToyLineManager Instance { get; private set; }
        public List<ToyLine> ToyLines = [];

        private void Awake() {
            Instance = this;
        }

        public void RegisterToyLine(ToyLine toyLine) {
            ToyLines.Add(toyLine);
        }

        public bool GetCollectedAllToyLines() {
            var progress = WinterProgress.Instance.LocalProgress;
            foreach(var toyLine in ToyLines) {
                if (!progress.IsToyLineCollected(toyLine.Guid))
                    return false;
            }
            return true;
        }

        public void RespawnAllToyLines() {
            foreach(var toyLine in ToyLines) {
                toyLine.Respawn();
            }
        }
    }
}
