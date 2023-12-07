using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    public class ToyMachine : MonoBehaviour {
        public float TimeInSecondsToSpray = 2f;
        public float PlayerExitSpeed = 20f;
        public Transform PlayerExitLocation = null;
        public float FadeInDuration = 0.2f;
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

        public void TeleportPlayerToExit(bool graffiti) {
            var player = WorldHandler.instance.GetCurrentPlayer();
            WorldHandler.instance.PlaceCurrentPlayerAt(PlayerExitLocation);
            player.SetVelocity(PlayerExitLocation.forward * PlayerExitSpeed);
            if (!graffiti) {
                var effects = Core.Instance.UIManager.effects;
                effects.FullBlackToFadeOut(0f, FadeInDuration);
            }
        }
    }
}
