 using UnityEngine;
using Winterland.MapStation.Common;
using Winterland.Plugin;

namespace Winterland.Common {
    public class DebugShapeDebugUI : MonoBehaviour {
        public static DebugShapeDebugUI Instance {get; private set;}

        public static void Create() {
            if(Instance != null) return;
            var gameObject = new GameObject($"Winterland {nameof(DebugShapeDebugUI)}");
            Instance = gameObject.AddComponent<DebugShapeDebugUI>();
            DontDestroyOnLoad(gameObject);
        }

        bool visible = WinterConfig.Instance.ShowRedDebugShapesValue;

        void Awake() {
            DebugUI.Instance.OnDebugUI += OnDebugUI;
        }

        private void OnDebugUI() {
            GUILayout.Label("Red Debug Shapes");
            if(WinterManager.Instance != null && WinterManager.Instance.stageAdditions != null) {
                if(GUILayout.Button((visible ? "Hide" : "Show") + " Red Debug Shapes")) {
                    visible = !visible;
                    DebugShapeUtility.SetDebugShapesVisibility(WinterManager.Instance.stageAdditions, visible);
                }
            }
        }
    }
}
