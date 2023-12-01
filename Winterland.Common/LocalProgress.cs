using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winterland.Common {
    public class LocalProgress : ILocalProgress {
        public WinterObjective Objective { get; set; }

        public LocalProgress() {
            Objective = ObjectiveDatabase.StartingObjective;
        }

        public void Save() {

        }
    }
}
