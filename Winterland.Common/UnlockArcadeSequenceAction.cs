using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public class UnlockArcadeSequenceAction : SequenceAction {
        [Header("Action to jump to when arcade is newly unlocked.")]
        public string SuccessfullyUnlockedTarget = "";
        [Header("Action to jump to when the arcade was already unlocked.")]
        public string AlreadyUnlockedTarget = "";

        public override void Run(bool immediate) {
            base.Run(immediate);
            var successAction = Sequence.Sequence.GetActionByName(SuccessfullyUnlockedTarget);
            var alreadyUnlockedAction = Sequence.Sequence.GetActionByName(AlreadyUnlockedTarget);
            var localProgress = WinterProgress.Instance.LocalProgress;
            if (localProgress.ArcadeUnlocked) {
                if (alreadyUnlockedAction != null)
                    alreadyUnlockedAction.Run(immediate);
                else
                    Finish(immediate);
                return;
            }
            localProgress.ArcadeUnlocked = true;
            localProgress.Save();
            ArcadeManager.Instance.UpdateArcades();
            if (successAction != null) {
                successAction.Run(immediate);
            } else
                Finish(immediate);
        }
    }
}
