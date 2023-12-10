using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winterland.Common {
    public class WinterProgress {
        public static WinterProgress Instance { get; private set; }
        public ILocalProgress LocalProgress = null;
        public IGlobalProgress GlobalProgress => WritableGlobalProgress;
        public WritableGlobalProgress WritableGlobalProgress = null;
        public WinterProgress() {
            Instance = this;
            LocalProgress = new LocalProgress();
#if WINTER_DEBUG
            if (WinterConfig.Instance.MockGlobalProgressLocallyValue) {
                WritableGlobalProgress = new MockGlobalProgress();
            } else {
                WritableGlobalProgress = new NetworkedGlobalProgress();
            }
#else
            WritableGlobalProgress = new NetworkedGlobalProgress();
#endif

#if WINTER_DEBUG
            if (!WinterConfig.Instance.ResetLocalSaveValue)
#endif
                LocalProgress.Load();
        }
    }
}
