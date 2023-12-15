using UnityEngine;
using SlopCrew.API;
using System.Runtime.Serialization;
using System;

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

        public delegate void OnPacketHandler(Packet packet);

        public OnPacketHandler OnPacket;

        private void Awake() {
            Instance = this;
            APIManager.OnAPIRegistered += OnSlopCrewAPIRegistered;
            var api = APIManager.API;
            if(api != null) {
                OnSlopCrewAPIRegistered(api);
            }
        }

        private void OnDestroy() {
            APIManager.OnAPIRegistered -= OnSlopCrewAPIRegistered;
            if(slopCrewApi != null) slopCrewApi.OnCustomPacketReceived -= onSlopCrewPacketReceived;
        }

        private void OnSlopCrewAPIRegistered(ISlopCrewAPI slopCrewApi) {
            if(this.slopCrewApi != null) return;
            this.slopCrewApi = slopCrewApi;
            slopCrewApi.OnCustomPacketReceived += onSlopCrewPacketReceived;
        }

        public void onSlopCrewPacketReceived(uint player, string id, byte[] data) {
            Debug.Log($"Received packet id {id} from player {player}. Data length is {data.Length}");
            Packet packet = PacketFactory.CreateBlankFromId(id);
            if (packet != null) {
                packet.PlayerID = player;
                try {
                    packet.Deserialize(data);
                } catch(PacketParseException e) {
                    Debug.Log(e.Message);
                    // Drop the packet, don't crash
                }
                DispatchReceivedPacket(packet);
            }
        }

        public void DispatchReceivedPacket(Packet packet) {
            Debug.Log($"Winterland packet received: {packet} {JsonUtility.ToJson(packet)}");
            OnPacket?.Invoke(packet);
        }

        public void SendPacket(Packet packet) {
            Debug.Log($"Sending Winterland packet: {packet} {JsonUtility.ToJson(packet)}");
            slopCrewApi.SendCustomPacket(packet.GetPacketId(), packet.Serialize());
        }

    }
}
