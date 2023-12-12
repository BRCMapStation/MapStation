using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winterland.Common {
    public class RespawnToyLinesSequenceAction : SequenceAction {
        public override void Run(bool immediate) {
            base.Run(immediate);
            var progress = WinterProgress.Instance.LocalProgress;
            ToyLineManager.Instance.RespawnAllToyLines();
            progress.Gifts++;
            progress.Save();
            Finish(immediate);
        }
    }
}
