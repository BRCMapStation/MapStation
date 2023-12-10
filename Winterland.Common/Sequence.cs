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
        public bool HidePlayer = false;
        public bool Skippable = true;
        public bool StandDownEnemies = true;
        public int DialogueLevelToAdd = 0;

        [Header("Events")]
        public WinterObjective SetObjectiveOnEnd;
        public string RunActionOnEnd;

        private SequenceWrapper actualSequence;
        private bool initialized = false;
        private SequenceAction[] actions;

        private void Initialize(CustomNPC npc) {
            if (initialized)
                return;
            initialized = true;
            actualSequence = new SequenceWrapper(this);
            actualSequence.NPC = npc;
            actions = SequenceAction.GetComponentsOrdered<SequenceAction>(gameObject);
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
            if (Application.isEditor)
                actions = gameObject.GetComponents<SequenceAction>();
            return actions;
        }

        public SequenceAction GetActionByName(string name) {
            if (Application.isEditor)
                actions = gameObject.GetComponents<SequenceAction>();
            if (string.IsNullOrEmpty(name))
                return null;
            foreach (var action in actions) {
                if (action.Name == name)
                    return action;
            }
            return null;
        }
    }
}
