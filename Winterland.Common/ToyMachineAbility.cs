using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using UnityEngine;

namespace Winterland.Common {
    public class ToyMachineAbility : Ability {
        private ToyMachine machine = null;
        private Vector3 forward = Vector3.zero;

        public ToyMachineAbility(Player player) : base(player) {

        }

        public override void Init() {
            normalMovement = false;
            normalRotation = false;
            customGravity = 0f;
            canStartGrind = false;
            canStartWallrun = false;
            treatPlayerAsSortaGrounded = true;
        }

        public void SetMachine(ToyMachine toyMachine, Vector3 machineForward) {
            machine = toyMachine;
            forward = machineForward;
            p.ActivateAbility(this);
            p.SetVelocity(machineForward * 10f);
            customVelocity = machineForward * 10f;
        }

        public override void FixedUpdateAbility() {
            p.SetVelocity(forward * 10f);
            customVelocity = forward * 10f;
            if (p.abilityTimer >= machine.TimeInSecondsToSpray) {
                p.StopCurrentAbility();
                machine.TeleportPlayerToExit(false);
            }
        }
    }
}
