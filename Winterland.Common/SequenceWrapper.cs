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

        public SequenceWrapper(Sequence sequence) {
            Sequence = sequence;
        }

        public override void Play() {
            base.Play();
            if (Sequence.ClearWantedLevel)
                WantedManager.instance.StopPlayerWantedStatus(false);

            foreach(var thing in Sequence.ActivateOnBegin) {
                thing.SetActive(true);
            }

            foreach(var thing in Sequence.DeactivateOnBegin) {
                thing.SetActive(false);
            }

            SetCamera(Sequence.Camera.gameObject);

            var actions = Sequence.GetActions();
            if (actions.Length > 0)
                actions[0].Run();
        }

        public override void Stop() {
            base.Stop();

            foreach (var thing in Sequence.ActivateOnEnd) {
                thing.SetActive(true);
            }

            foreach (var thing in Sequence.DeactivateOnEnd) {
                thing.SetActive(false);
            }

            if (Sequence.SetObjectiveOnEnd != null) {
                WinterProgress.Instance.LocalProgress.Objective = Sequence.SetObjectiveOnEnd;
                WinterProgress.Instance.Save();
            }
        }
    }
}
