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

        string eventProgressTreePercentageString;
        private void OnDebugUI() {
            eventProgressTreePercentageString = GUILayout.TextField(eventProgressTreePercentageString);
            if (GUILayout.Button("Simulate receiving event progress packet")) {
                NetManager.Instance.DispatchPacket(new EventProgressPacket {
                    PlayerID = 0,
                    treeGrowthPercentage = UInt16.Parse(eventProgressTreePercentageString)
                });
            }
        }
    }
}