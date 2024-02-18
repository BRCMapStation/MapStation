using UnityEngine;
using MapStation.Common;
using cspotcode.SlopCrewClient;
using ProtoBuf;

namespace MapStation.Plugin.Tools;

[ProtoContract]
[ProtoInclude(1, typeof(PlayAnimation))]
class BasePacket {}

[ProtoContract]
class PlayAnimation : BasePacket {
    [ProtoMember(1)]
    public string directorId;
    [ProtoMember(2)]
    public Vector3 position;
}

class SlopCrewClientUI : DebugUI.DebugMenu {
    public override string Name => "SlopCrewClient";

    private Client<BasePacket> client;

    public SlopCrewClientUI() {
        client = new Client<BasePacket>("MapStation.Common");
        client.Enable();
        client.OnPacketReceived += (player, packet, isLocal) => {
            Debug.Log($"{player} sent us: {packet}");
            if (packet is PlayAnimation p) {
                Debug.Log(p.directorId);
                Debug.Log(p.position);
            }
        };
    }

    public override void OnGUI() {
        GUILayout.Label($"IsSlopCrewInstalled {cspotcode.SlopCrewClient.SlopCrewAPI.APIManager.IsSlopCrewInstalled}");
        GUILayout.Label($"API available? {client.ApiAvailable}");
        if (client.ApiAvailable) {
            GUILayout.Label($"Connected {client.SlopCrewAPI.Connected}");
            GUILayout.Label($"TickRate {client.SlopCrewAPI.TickRate}");
            GUILayout.Label($"ServerAddress {client.SlopCrewAPI.ServerAddress}");
            GUILayout.Label($"Latency {client.SlopCrewAPI.Latency}");
            GUILayout.Label($"PlayerCount {client.SlopCrewAPI.PlayerCount}");
            foreach (var id in client.SlopCrewAPI.Players) {
                GUILayout.Label($"Player#{id}: {client.SlopCrewAPI.GetPlayerName(id)}");
            }
        }
        GUILayout.Label($"CurrentTick {client.CurrentTick}");
        GUILayout.Label($"CurrentTickSmoothed {client.CurrentTickSmoothed}");
        GUILayout.Label($"TickDuration {client.TickDuration}");
        var localAhead = client.CurrentTickSmoothed > client.CurrentTick;
        var serverAhead = client.CurrentTickSmoothed < client.CurrentTick;
        var difference = localAhead ? client.CurrentTickSmoothed - client.CurrentTick : client.CurrentTick - client.CurrentTickSmoothed;
        GUILayout.Label($"Tick difference {difference} {(localAhead ? "Local is ahead" : serverAhead ? "Server is ahead" : "")}");
        if (GUILayout.Button("Send Packet")) {
            client.Send(new PlayAnimation() {
                directorId = "foobar",
                position = Vector3.down
            }, true);
        }
    }
}
