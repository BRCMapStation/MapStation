using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlopCrew.Server.XmasEvent;

namespace Winterland.Common {
    /// <summary>
    /// Serverside progress.
    /// </summary>
    public interface IGlobalProgress {
        XmasServerEventStatePacket State { get; }
        delegate void OnGlobalStageChangedHandler();
        event OnGlobalStageChangedHandler OnGlobalStateChanged;
    }

    public class WritableGlobalProgress : IGlobalProgress {
        public XmasServerEventStatePacket State { get; private set; }

        public event IGlobalProgress.OnGlobalStageChangedHandler OnGlobalStateChanged;

        public void SetState(XmasServerEventStatePacket state) {
            State = state;
            this.OnGlobalStateChanged?.Invoke();
        }
    }
}
