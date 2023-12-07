using UnityEngine;
using SlopCrew.API;
using System.Runtime.Serialization;

namespace Winterland.Common {
    /// <summary>
    /// Global tree controller, instantiated on all stages.
    /// Tracks networked tree state at all times.
    /// </summary>
    public class NetManager : MonoBehaviour {
        public static NetManager Instance {get; private set;}

        public static NetManager Create() {
            var go = new GameObject($"Winter {nameof(NetManager)}");
            DontDestroyOnLoad(go);
            return go.AddComponent<NetManager>();
        }

        private ISlopCrewAPI slopCrewApi;

        public delegate void OnPacketDelegate(Packet packet);

        public OnPacketDelegate OnPacket;

        private void Awake() {
            Instance = this;
            APIManager.OnAPIRegistered += OnSlopCrewAPIRegistered;
            var api = APIManager.API;
            if(api != null)
                OnSlopCrewAPIRegistered(api);
        }

        private void OnDestroy() {
            APIManager.OnAPIRegistered -= OnSlopCrewAPIRegistered;
            if(slopCrewApi != null) slopCrewApi.OnCustomPacketReceived -= OnCustomPacketReceived;
        }

        private void OnSlopCrewAPIRegistered(ISlopCrewAPI slopCrewApi) {
            if(this.slopCrewApi != null) return;
            this.slopCrewApi = slopCrewApi;
            slopCrewApi.OnCustomPacketReceived += OnCustomPacketReceived;
        }

        private void OnCustomPacketReceived(uint player, string id, byte[] data) {
            Packet packet = null;
            switch(id) {
                case PlayerCollectGiftsPacket.PacketId:
                    packet = new PlayerCollectGiftsPacket();
                    break;
                case EventProgressPacket.PacketId:
                    packet = new EventProgressPacket();
                    break;
            }
            if (packet != null) {
                packet.Deserialize(data);
                OnPacket?.Invoke(packet);
            }
        }
    }
}
