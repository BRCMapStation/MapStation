using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using UnityEngine;

namespace Winterland.Common {
    public class ToyMachineAbility : Ability {
        public bool TaggedAlready = false;
        public bool CanTag = true;
        private ToyMachine machine = null;
        private Vector3 forward = Vector3.zero;
        private bool fadingOut = false;

        public ToyMachineAbility(Player player) : base(player) {

        }

        public override void Init() {
            normalMovement = false;
            normalRotation = false;
            customGravity = 0f;
            canStartGrind = false;
            canStartWallrun = false;
            treatPlayerAsSortaGrounded = true;
            canUseSpraycan = false;
        }

        public void SetMachine(ToyMachine toyMachine, Vector3 machineForward) {
            machine = toyMachine;
            forward = machineForward;
            p.ActivateAbility(this);
            p.SetVelocity(machineForward * 10f);
            customVelocity = machineForward * 10f;
        }

        public override void OnStartAbility() {
            TaggedAlready = false;
            CanTag = true;
            fadingOut = false;
        }

        public override void FixedUpdateAbility() {
            var actualDuration = machine.TimeInSecondsToSpray + machine.FadeOutDuration + machine.BlackScreenDuration;
            var winterPlayer = WinterPlayer.Get(p);
            if (!fadingOut && p.abilityTimer >= machine.TimeInSecondsToSpray) {
                var shouldTag = winterPlayer.CurrentToyLine != null && winterPlayer.CollectedToyParts == winterPlayer.CurrentToyLine.ToyParts.Length && !TaggedAlready;
                if (shouldTag) {
                    TaggedAlready = true;
                    var gSpot = machine.GetComponentInChildren<GraffitiSpot>();
                    if (gSpot != null) {
                        p.StartGraffitiMode(gSpot);
                        return;
                    }
                }
                CanTag = false;
                fadingOut = true;
                var effects = Core.Instance.UIManager.effects;
                effects.FadeToBlack(machine.FadeOutDuration);
            }
            p.SetVelocity(forward * 10f);
            customVelocity = forward * 10f;

            if (p.abilityTimer >= actualDuration) {
                p.StopCurrentAbility();
                winterPlayer.DropCurrentToyLine();
                machine.TeleportPlayerToExit(false);
                machine.PlayFailureSound();
            }
        }
    }
}
