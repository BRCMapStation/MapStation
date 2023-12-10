using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winterland.Common {
    /// <summary>
    /// Local, not networked global progress, for testing.
    /// </summary>
    public class MockGlobalProgress : WritableGlobalProgress {
        public MockGlobalProgress() {
            SetTreeConstructionPercentage(WinterConfig.Instance.MockGlobalProgressStartTreeAtValue);
        }
    }
}
