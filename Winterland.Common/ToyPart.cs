using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    public class ToyPart : MonoBehaviour {
        public ToyLine Line { get; private set; }

        private void OnTriggerStay(Collider other) {
            var player = other.GetComponentInChildren<Player>();
            if (!player)
                player = other.GetComponentInParent<Player>();
            if (!player)
                return;
            if (player.IsDead())
                return;
            if (player.isAI)
                return;
            if (player.IsBusyWithSequence())
                return;
            if (!player.IsComboing())
                return;
            var winterPlayer = WinterPlayer.Get(player);
            if (winterPlayer == null)
                return;
            if (winterPlayer.CurrentToyLine != null) {
                if (winterPlayer.CurrentToyLine != Line)
                    return;
            }
            Collect(player);
        }

        public void Respawn() {
            gameObject.SetActive(true);
        }

        private void Collect(Player player) {
            var winterPlayer = WinterPlayer.Get(player);
            if (winterPlayer.CurrentToyLine != Line)
                winterPlayer.CollectedToyParts = 0;
            winterPlayer.CurrentToyLine = Line;
            winterPlayer.CollectedToyParts++;
            if (WinterUI.Instance != null && winterPlayer.Local) {
                var toyLineUI = WinterUI.Instance.ToyLineUI;
                toyLineUI.Visible = true;
                toyLineUI.SetCounter(winterPlayer.CollectedToyParts, Line.ToyParts.Length);
            }
            gameObject.SetActive(false);
        }

        private void Awake() {
            Line = GetComponentInParent<ToyLine>(true);
        }
    }
}
