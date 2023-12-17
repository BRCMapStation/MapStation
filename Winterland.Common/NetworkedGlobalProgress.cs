using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlopCrew.Server.XmasEvent;

namespace Winterland.Common {
    public class NetworkedGlobalProgress : WritableGlobalProgress, IDisposable {
        public NetworkedGlobalProgress() {
            NetManager.Instance.OnPacket += OnPacket;
        }

        public void Dispose() {
            NetManager.Instance.OnPacket -= OnPacket;
        }

        public void OnPacket(XmasPacket packet) {
            if(packet is XmasServerEventStatePacket p) {
                this.SetState(p);
            }
        }
    }
}
