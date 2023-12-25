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

#if WINTER_DEBUG
        bool visible = WinterConfig.Instance.ShowRedDebugShapesValue;
#else
        bool visible = false;
#endif

        void Awake() {
            DebugUI.Instance.RegisterMenu("Debug Shapes", OnDebugUI);
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
