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
            GUILayout.Label("Tree State");
            var t = TreeController.Instance;
            if(!t) {
                GUILayout.Label("<tree does not exist>");
                return;
            }
            GUILayout.Label($"Active phase: {t.activePhase?.gameObject.name}");
            foreach(var phase in t.treePhases) {
                GUILayout.Label($"Phase {phase.gameObject.name} progress = {phase.Progress}");
            }
            GUILayout.Label($"ReasonsToBePaused.Count = {t.ReasonsToBePaused.Count}");
            foreach(var part in t.treeParts) {
                if(part.animator != null) {
                    GUILayout.Label($"Part {part.gameObject.name} animator params: {summarizeAnimatorParams(part.animator)}");
                    // I tried to log animator state names, but apparently you can't do that easily?
                    // GUILayout.Label($"{part.animator.GetCurrentAnimatorStateInfo(0).nameHash}->{part.animator.GetNextAnimatorStateInfo(0).nameHash}");
                }
            }
        }
        
        private string summarizeAnimatorParams(Animator animator) {
            var summary = "";
            foreach(var p in animator.parameters) {
                if(animator.GetBoolString(p.name)) {
                    summary += $"{p.name},";
                }
            }
            return summary.ToString().TrimEnd(',');
        }
    }
}
