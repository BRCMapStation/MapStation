using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;
using CommonAPI;

namespace Winterland.Common {
    public class Sequence : MonoBehaviour {
        [Header("General")]
        public CameraRegisterer Camera;
        public bool ClearWantedLevel = true;
        public bool HidePlayer = false;
        public bool Skippable = true;
        [Header("On Begin")]
        public GameObject[] ActivateOnBegin;
        public GameObject[] DeactivateOnBegin;
        [Header("On End")]
        public GameObject[] ActivateOnEnd;
        public GameObject[] DeactivateOnEnd;
        public WinterObjective SetObjectiveOnEnd;

        private SequenceWrapper actualSequence;
        private bool initialized = false;
        private SequenceAction[] actions;

        private void Initialize(CustomNPC npc) {
            if (initialized)
                return;
            initialized = true;
            actualSequence = new SequenceWrapper(this);
            actions = gameObject.GetComponents<SequenceAction>();
            for(var i = 0; i < actions.Length; i++) {
                var action = actions[i];
                action.NPC = npc;
                action.Sequence = actualSequence;
                if (i < actions.Length - 1) {
                    action.NextAction = actions[i + 1];
                }
            }
        }

        public SequenceWrapper GetCustomSequence(CustomNPC npc) {
            Initialize(npc);
            return actualSequence;
        }

        public SequenceAction[] GetActions() {
            return actions;
        }
    }
}
