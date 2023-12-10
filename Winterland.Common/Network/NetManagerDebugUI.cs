using UnityEngine;
using SlopCrew.API;
using System.Runtime.Serialization;
using System;

namespace Winterland.Common {
    public class NetManagerDebugUI : MonoBehaviour {
        public static NetManagerDebugUI Instance {get; private set;}

        float TreeConstructionPercentageSlider;

        public static void Create() {
            if(Instance != null) return;
            var gameObject = new GameObject($"Winterland {nameof(NetManagerDebugUI)}");
            Instance = gameObject.AddComponent<NetManagerDebugUI>();
            DontDestroyOnLoad(gameObject);
        }

        void Awake() {
            DebugUI.Instance.OnDebugUI += OnDebugUI;
        }

        private void OnDebugUI() {
            GUILayout.Label("Progress is measured from 0 to 1, a decimal / float");
            GUILayout.Label($"Using {WinterProgress.Instance.GlobalProgress.GetType().Name}");
            GUILayout.Label($"Current {nameof(EventProgressPacket.TreeConstructionPercentage)} = {WinterProgress.Instance.GlobalProgress.TreeConstructionPercentage}");
            GUILayout.BeginHorizontal();
            GUILayout.Label(nameof(EventProgressPacket.TreeConstructionPercentage));
            TreeConstructionPercentageSlider = GUILayout.HorizontalSlider(TreeConstructionPercentageSlider, 0, 1);
            GUILayout.EndHorizontal();
            GUILayout.Label($"{TreeConstructionPercentageSlider}");
            if (GUILayout.Button("Set event progress locally")) {
                WinterProgress.Instance.WritableGlobalProgress.SetTreeConstructionPercentage(TreeConstructionPercentageSlider);
            }
            if (GUILayout.Button("Simulate receiving event progress packet")) {
                NetManager.Instance.DispatchReceivedPacket(createEventProgressPacket());
            }
            GUILayout.Label("Broadcasting event progress will affect ALL WINTERLAND TESTERS!");
            if (GUILayout.Button("BROADCAST EVENT PROGRESS TO ALL PLAYERS")) {
                var packet = createEventProgressPacket();
                NetManager.Instance.SendPacket(packet);
                NetManager.Instance.DispatchReceivedPacket(packet);
            }
        }

        private EventProgressPacket createEventProgressPacket() {
            return new EventProgressPacket {
                PlayerID = 0,
                TreeConstructionPercentage = TreeConstructionPercentageSlider
            };
        }
    }
}