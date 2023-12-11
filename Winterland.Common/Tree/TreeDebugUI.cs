using UnityEngine;

namespace Winterland.Common {
    public class TreeDebugUI : MonoBehaviour {
        public static TreeDebugUI Instance {get; private set;}

        public static void Create() {
            if(Instance != null) return;
            var gameObject = new GameObject($"Winterland {nameof(TreeDebugUI)}");
            Instance = gameObject.AddComponent<TreeDebugUI>();
            DontDestroyOnLoad(gameObject);
        }

        void Awake() {
            DebugUI.Instance.OnDebugUI += OnDebugUI;
        }

        private void OnDebugUI() {
            GUILayout.Label("Tree state");
            var t = TreeController.Instance;
            if(!t) {
                GUILayout.Label("<tree does not exist>");
                return;
            }
            GUILayout.Label($"Active phase: {t.activePhase?.gameObject.name}");
            foreach(var phase in t.treePhases) {
                GUILayout.Label($"Phase {phase.gameObject.name} progress = {phase.Progress}");
            }
        }
    }
}