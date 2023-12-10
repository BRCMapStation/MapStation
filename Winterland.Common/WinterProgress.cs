using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winterland.Common {
    public class WinterProgress {
        public static WinterProgress Instance { get; private set; }
        public ILocalProgress LocalProgress = null;
        public IGlobalProgress GlobalProgress = null;
        public WinterProgress() {
            Instance = this;
            LocalProgress = new LocalProgress();
            GlobalProgress = new MockGlobalProgress();

#if WINTER_DEBUG
            if (!WinterConfig.Instance.ResetLocalSaveValue)
#endif
                LocalProgress.Load();
        }
    }
}
