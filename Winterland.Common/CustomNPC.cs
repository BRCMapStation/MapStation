using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CommonAPI;
using Reptile;

namespace Winterland.Common {
    [ExecuteAlways]
    public class CustomNPC : MonoBehaviour {
        [Header("General")]
        public string Name = "";
        public bool PlacePlayerAtSnapPosition = true;
        public bool LookAt = true;
        public bool ShowRep = false;
        public int MaxDialogueLevel = 1;
        [HideInInspector]
        public int CurrentDialogueLevel = 0;
        private EventDrivenInteractable interactable;
        private DialogueBranch[] dialogueBranches;

        private void Awake() {
            if (Application.isEditor)
                return;
            ReptileAwake();

            void ReptileAwake() {
                CurrentDialogueLevel = 0;
                dialogueBranches = GetComponentsInChildren<DialogueBranch>();
                interactable = gameObject.AddComponent<EventDrivenInteractable>();
                interactable.OnInteract = Interact;
                interactable.PlacePlayerAtSnapPosition = PlacePlayerAtSnapPosition;
                interactable.ShowRep = ShowRep;
                interactable.LookAt = LookAt;
            }
        }

        private void OnDestroy() {
            if (!Application.isEditor)
                return;
            var branches = GetComponentsInChildren<DialogueBranch>();
            foreach(var branch in branches) {
                DestroyImmediate(branch);
            }
        }

        public void AddDialogueLevel(int dialogueLevelToAdd) {
            CurrentDialogueLevel += dialogueLevelToAdd;
            CurrentDialogueLevel = Mathf.Clamp(CurrentDialogueLevel, 0, MaxDialogueLevel);
        }

        public void Interact(Player player) {
            Sequence sequence = null;
            foreach (var branch in dialogueBranches) {
                if (branch.Test(this)) {
                    sequence = branch.Sequence;
                    break;
                }
            }
            if (sequence == null)
                return;
            var sequenceWrapper = sequence.GetCustomSequence(this);
            if (sequenceWrapper == null)
                return;
            interactable.StartEnteringSequence(sequenceWrapper, sequence.HidePlayer, true, false, true, true, sequence.Skippable, true, false);
        }
    }
}
