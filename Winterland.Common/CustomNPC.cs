using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CommonAPI;
using Reptile;

namespace Winterland.Common {
    public class CustomNPC : MonoBehaviour {
        public bool PlacePlayerAtSnapPosition = true;
        public bool LookAt = true;
        public bool ShowRep = false;
        public string Name = "";
        private EventDrivenInteractable interactable;
        private DialogueBranch[] dialogueBranches;

        private void Awake() {
            dialogueBranches = GetComponentsInChildren<DialogueBranch>();
            interactable = gameObject.AddComponent<EventDrivenInteractable>();
            interactable.OnInteract = Interact;
            interactable.PlacePlayerAtSnapPosition = PlacePlayerAtSnapPosition;
            interactable.ShowRep = ShowRep;
            interactable.LookAt = LookAt;
        }

        public void Interact(Player player) {
            Sequence sequence = null;
            foreach (var branch in dialogueBranches) {
                if (branch.Test()) {
                    sequence = branch.Sequence;
                    break;
                }
            }
            if (sequence == null)
                return;
            var sequenceWrapper = sequence.GetCustomSequence();
            if (sequenceWrapper == null)
                return;
            interactable.StartEnteringSequence(sequenceWrapper, sequence.HidePlayer, true, false, true, true, sequence.Skippable, true, false);
        }
    }
}
