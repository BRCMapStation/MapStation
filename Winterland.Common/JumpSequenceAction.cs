using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public class JumpSequenceAction : SequenceAction {
        [Header("Target action to jump to.")]
        public string Target = "";
        public override void Run(bool immediate) {
            base.Run(immediate);
            var targetAction = Sequence.Sequence.GetActionByName(Target);
            if (targetAction != null) {
                targetAction.Run(immediate);
            } else
                Finish(immediate);
        }
    }
}
