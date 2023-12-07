using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winterland.Common {
    public class EndSequenceAction : SequenceAction {
        public override bool SequenceOnly => true;
        public override void Run() {
            Sequence.ExitSequence();
        }
    }
}
