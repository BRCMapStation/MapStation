using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Winterland.Common.Challenge;

namespace Winterland.Common {
    public class QuitChallengeSequenceAction : SequenceAction {
        public override void Run(bool immediate) {
            base.Run(immediate);
            var currentChallenge = ChallengeLevel.CurrentChallengeLevel;
            if (currentChallenge == null) {
                Finish(immediate);
                return;
            }
            currentChallenge.QuitChallenge();
            Finish(immediate);
        }
    }
}
