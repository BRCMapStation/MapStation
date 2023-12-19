using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winterland.Common.Challenge;

namespace Winterland.Common {
    public class StartChallengeSequenceAction : SequenceAction {
        public ChallengeLevel ChallengeToStart = null;
        public override void Run(bool immediate) {
            base.Run(immediate);
            ChallengeToStart.StartChallenge();
            Finish(immediate);
        }
    }
}
