using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    public class ToyMachine : MonoBehaviour{
        // TODO: Sounds, polish and all.
        public void FinishToyLine() {
            var player = WorldHandler.instance.GetCurrentPlayer();
            var winterPlayer = WinterPlayer.Get(player);
            if (winterPlayer == null)
                return;
            if (winterPlayer.CurrentToyLine == null)
                return;
            if (winterPlayer.CollectedToyParts != winterPlayer.CurrentToyLine.ToyParts.Length)
                winterPlayer.DropCurrentToyLine();
            else
                winterPlayer.FinishCurrentToyLine();
        }
    }
}
