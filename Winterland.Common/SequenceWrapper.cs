using CommonAPI;
using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winterland.Common {
    public class SequenceWrapper : CustomSequence {
        public Sequence Sequence;
        public CustomNPC NPC;
        public string CurrentActionToRunOnEnd;

        public SequenceWrapper(Sequence sequence) {
            Sequence = sequence;
        }

        public override void Play() {
            base.Play();

            NPC.CurrentDialogueLevel += Sequence.DialogueLevelToAdd;

            CurrentActionToRunOnEnd = Sequence.RunActionOnEnd;

            if (Sequence.ClearWantedLevel)
                WantedManager.instance.StopPlayerWantedStatus(false);

            var actions = Sequence.GetActions();
            if (actions.Length > 0)
                actions[0].Run(false);
        }

        public override void Stop() {
            base.Stop();

            if (Sequence.SetObjectiveOnEnd != null) {
                WinterProgress.Instance.LocalProgress.Objective = Sequence.SetObjectiveOnEnd;
                WinterProgress.Instance.LocalProgress.Save();
            }

            if (!string.IsNullOrEmpty(CurrentActionToRunOnEnd)) {
                var action = Sequence.GetActionByName(CurrentActionToRunOnEnd);
                if (action != null) {
                    action.Run(true);
                }
            }
        }
    }
}
