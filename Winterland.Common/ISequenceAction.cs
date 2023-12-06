using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Winterland.Common {
    public interface ISequenceAction {
        public IEnumerator Run(SequenceWrapper sequence, CustomNPC npc);
    }
}
