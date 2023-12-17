using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlopCrew.Server.XmasEvent;

namespace Winterland.Common {
    /// <summary>
    /// Local, not networked global progress, for testing.
    /// </summary>
    public class MockGlobalProgress : WritableGlobalProgress {
        public MockGlobalProgress() {
            SetState(new XmasServerEventStatePacket());
        }
    }
}
