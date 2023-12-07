using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    public class ToyMachineFreeze : MonoBehaviour {
        private ToyMachine owner;
        private void Awake() {
            owner = GetComponentInParent<ToyMachine>();
        }

        public void Activate(Player player) {
            if (player.ability is ToyMachineAbility)
                return;
            if (owner == null)
                return;
            if (player.isAI)
                return;
            var winterPlayer = WinterPlayer.Get(player);
            if (winterPlayer == null)
                return;
            var toyMachineAbility = winterPlayer.ToyMachineAbility;
            if (toyMachineAbility == null)
                return;
            owner.PlayEnterToyMachineSound();
            toyMachineAbility.SetMachine(owner, transform.forward);
        }
    }
}
