using UnityEngine;
using SlopCrew.Server.XmasEvent;

namespace Winterland.Common {
    public class NetManagerDebugUI : MonoBehaviour {
        public static NetManagerDebugUI Instance {get; private set;}

        float treeConstructionPercentageSlider;

        public static void Create() {
            if(Instance != null) return;
            var gameObject = new GameObject($"Winterland {nameof(NetManagerDebugUI)}");
            Instance = gameObject.AddComponent<NetManagerDebugUI>();
            DontDestroyOnLoad(gameObject);
        }

        void Awake() {
            DebugUI.Instance.RegisterMenu("Net Manager", OnDebugUI);
        }

        private void OnDebugUI() {
            GUILayout.Label("Progress is measured from 0 to 1, a decimal / float");
            GUILayout.Label($"Using {WinterProgress.Instance.GlobalProgress.GetType().Name}");
            GUILayout.Label($"Current {nameof(XmasServerEventProgressPacket.TreeConstructionPercentage)} = {WinterProgress.Instance.GlobalProgress.TreeConstructionPercentage}");
            GUILayout.BeginHorizontal();
            GUILayout.Label(nameof(XmasServerEventProgressPacket.TreeConstructionPercentage));
            treeConstructionPercentageSlider = GUILayout.HorizontalSlider(treeConstructionPercentageSlider, 0, 1);
            GUILayout.EndHorizontal();
            GUILayout.Label($"{treeConstructionPercentageSlider}");
            if (GUILayout.Button("Set event progress locally")) {
                WinterProgress.Instance.WritableGlobalProgress.SetTreeConstructionPercentage(treeConstructionPercentageSlider);
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

        private XmasServerEventProgressPacket createEventProgressPacket() {
            return new XmasServerEventProgressPacket {
                PlayerID = 0,
                TreeConstructionPercentage = treeConstructionPercentageSlider
            };
        }
    }
}
