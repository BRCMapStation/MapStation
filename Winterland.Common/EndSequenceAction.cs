using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winterland.Common {
    public class EndSequenceAction : SequenceAction {
        public override void Run(bool immediate) {
            base.Run(immediate);
            if (immediate)
                return;
            Sequence.ExitSequence();
        }
    }
}
