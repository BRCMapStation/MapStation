using UnityEngine;
using SlopCrew.API;
using System.Runtime.Serialization;
using System;
using BepInEx.Logging;
using SlopCrew.Server.XmasEvent;

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

        private ManualLogSource PacketLogger;
        private ISlopCrewAPI slopCrewApi;

        public delegate void OnPacketHandler(XmasPacket packet);

        public OnPacketHandler OnPacket;

        private void Awake() {
            Instance = this;
            this.PacketLogger = WinterLogging.CreateLogger(nameof(NetManager), onlyForDebugBuild: true);
            
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
            this.PacketLogger.LogInfo($"Received packet id {id} from player {player}. Data length is {data.Length}");
            XmasPacket packet = null;
            try {
                packet = XmasPacketFactory.ParsePacket(player, id, data);
            } catch(XmasPacketParseException e) {
                Debug.Log(e.Message);
                // Drop the packet, don't crash
            }
            if (packet != null) {
                DispatchReceivedPacket(packet);
            }
        }

        public void DispatchReceivedPacket(XmasPacket packet) {
            this.PacketLogger.LogInfo($"Winterland packet received: {packet} {packet.Describe()}");
            OnPacket?.Invoke(packet);
        }

        public void SendPacket(XmasPacket packet) {
            this.PacketLogger.LogInfo($"Sending Winterland packet: {packet} {packet.Describe()}");
            slopCrewApi.SendCustomPacket(packet.GetPacketId(), packet.Serialize());
        }
    }
}
