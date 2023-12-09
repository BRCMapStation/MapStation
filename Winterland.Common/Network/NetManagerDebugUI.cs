using UnityEngine;
using SlopCrew.API;
using System.Runtime.Serialization;
using System;

namespace Winterland.Common {
    public class NetManagerDebugUI : MonoBehaviour {
        public static NetManagerDebugUI Instance {get; private set;}

        public static void Create() {
            if(Instance != null) return;
            var gameObject = new GameObject($"Winterland {nameof(NetManagerDebugUI)}");
            Instance = gameObject.AddComponent<NetManagerDebugUI>();
            DontDestroyOnLoad(gameObject);
        }

        void Awake() {
            DebugUI.Instance.OnDebugUI += OnDebugUI;
        }

        string eventProgressTreePercentageString;
        private void OnDebugUI() {
            GUILayout.BeginHorizontal();
            GUILayout.Label(nameof(EventProgressPacket.treeGrowthPercentage));
            eventProgressTreePercentageString = GUILayout.TextField(eventProgressTreePercentageString);
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Simulate receiving event progress packet")) {
                var packet = new EventProgressPacket {
                    PlayerID = 0,
                    treeGrowthPercentage = UInt16.Parse(eventProgressTreePercentageString)
                };
                NetManager.Instance.DispatchPacket(packet);
            }
        }
    }
}