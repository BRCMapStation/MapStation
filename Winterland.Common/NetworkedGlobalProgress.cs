using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winterland.Common {
    public class NetworkedGlobalProgress : WritableGlobalProgress, IDisposable {
        public NetworkedGlobalProgress() {
            NetManager.Instance.OnPacket += OnPacket;
        }

        public void Dispose() {
            NetManager.Instance.OnPacket -= OnPacket;
        }

        public void OnPacket(Packet packet) {
            if(packet is ServerEventProgressPacket p) {
                WinterProgress.Instance.WritableGlobalProgress.SetTreeConstructionPercentage(p.TreeConstructionPercentage);
            }
        }
    }
}
