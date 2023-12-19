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

        private TreeProgress uiSelectedProgress = new ();

        void Awake() {
            DebugUI.Instance.RegisterMenu("Tree", OnDebugUI);
        }

        private void OnDebugUI() {
            GUILayout.Label("Tree State");
            var t = TreeController.Instance;
            if(!t) {
                GUILayout.Label("<tree does not exist>");
                return;
            }
            var state = "";
            if (t.TargetProgress != null) {
                state += $"Target phase: #{t.TargetProgress.ActivePhaseIndex} at {(int)(t.TargetProgress.ActivePhaseProgress * 100)}% complete\n";
            }
            state += "All phases, current is >:\n";
            // TODO does not show animator params!  I hope we don't use these!
            for(var i = 0; i < t.treePhases.Length; i++) {
                var isActivePhase = i == t.CurrentProgress.ActivePhaseIndex;
                state += $"#{i} {(isActivePhase ? "> " : " ")} {t.treePhases[i].gameObject.name} {(isActivePhase ? $" - {(int)(t.CurrentProgress.ActivePhaseProgress * 100)}% complete" : "")}\n";
            }
            GUILayout.TextArea(state);
            var manualModeBefore = !t.SyncToGlobalProgress;
            var manualMode = GUILayout.Toggle(manualModeBefore, "Test tree animations manually");
            if (manualMode != manualModeBefore) {
                t.SyncToGlobalProgress = !manualMode;
                // Reset immediately, more intuitive this way
                if (manualMode) {
                    t.TargetProgress = this.uiSelectedProgress.Clone();
                    t.ResetToTarget();
                }
            }
            if(!t.SyncToGlobalProgress) {
                GUILayout.Label("Choose phase and percent complete, then click button to apply");
                for (var i = 0; i < t.treePhases.Length; i++) {
                    if (GUILayout.Button($"#{i} {(this.uiSelectedProgress.ActivePhaseIndex == i ? ">" : "")} {t.treePhases[i].gameObject.name}")) {
                        this.uiSelectedProgress.ActivePhaseIndex = i;
                    }
                }
                GUILayout.Label("Phase progress");
                this.uiSelectedProgress.ActivePhaseProgress =
                    GUILayout.HorizontalSlider(this.uiSelectedProgress.ActivePhaseProgress, 0, 1);

                GUILayout.BeginHorizontal();
                if(GUILayout.Button("Reset")) {
                    t.TargetProgress = this.uiSelectedProgress.Clone();
                    t.ResetToTarget();
                }
                var wouldRewind = t.TargetProgress.IsPrecededBy(uiSelectedProgress);
                GUI.enabled = !wouldRewind;
                if(GUILayout.Button("Animate")) {
                    // Allow tree's normal animation to move towards new target
                    t.TargetProgress = this.uiSelectedProgress.Clone();
                }
                GUI.enabled = true;
                GUILayout.EndHorizontal();
            }
            GUILayout.Label($"{nameof(t.ConstructionBlockers)}.Count = {t.ConstructionBlockers.Count}");
            // foreach(var part in t.treeParts) {
            //     if(part.animator != null) {
            //         GUILayout.Label($"Part {part.gameObject.name} animator params: {summarizeAnimatorParams(part.animator)}");
            //         // I tried to log animator state names, but apparently you can't do that easily?
            //         // GUILayout.Label($"{part.animator.GetCurrentAnimatorStateInfo(0).nameHash}->{part.animator.GetNextAnimatorStateInfo(0).nameHash}");
            //     }
            // }
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
