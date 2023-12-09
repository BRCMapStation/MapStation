using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public class SetActionToRunOnEndAction : SequenceAction {
        [Header("Sets an action to run when the sequence finishes or is skipped.")]
        public string ActionToRunOnEnd = "";

        public override void Run(bool immediate) {
            base.Run(immediate);
            Sequence.CurrentActionToRunOnEnd = ActionToRunOnEnd;
            Finish(immediate);
        }
    }
}
