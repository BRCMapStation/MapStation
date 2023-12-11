using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Winterland.Common {
    /// <summary>
    /// Exists only on the Square stage, connects to the TreeManager and drives the tree's animations, visuals, behavior.
    /// </summary>
    public class TreeController : MonoBehaviour, ITreeState {
        public static TreeController Instance { get; private set; }

        [SerializeField]
        public PlayableDirector director = null;

        private const float MinTime = 0f;
        [HideInInspector]
        private float maxStepSpeed = 0.01f;
        [HideInInspector]
        private float maxStepSize = 0.01f;

        [SerializeField]
        private TreePart[] treeParts;
        [SerializeField]
        public TreePhase[] treePhases;

        private int activePhaseIndex = -1;

        public TreePhase activePhase => activePhaseIndex >= 0 && activePhaseIndex < treePhases.Length ? treePhases[activePhaseIndex] : null;
        public TreePhase nextPhase => activePhaseIndex >= -1 && activePhaseIndex < treePhases.Length - 1 ? treePhases[activePhaseIndex + 1] : null;

        public bool isFastForwarding {
            get => isFastForwarding;
            private set {
                isFastForwarding = value;
                foreach(var treePart in treeParts) {
                    treePart.animator?.SetBoolString("IsFastForwarding", value);
                }
            }
        }

        [HideInInspector]
        public HashSet<ITreePauseReason> reasonsToBePaused = new ();
        public HashSet<ITreePauseReason> ReasonsToBePaused => reasonsToBePaused;

        private TimelineScrubber timeline;

        void OnValidate() {
            treeParts = GetComponentsInChildren<TreePart>();
            treePhases = GetComponentsInChildren<TreePhase>();
            if(director == null) director = GetComponent<PlayableDirector>();
#if UNITY_EDITOR
            EditorOnValidate();
#endif
        }

        void Awake() {
            Instance = this;

            foreach(var treePart in treeParts) {
                treePart.state = this;
                treePart.ResetPart();
            }
            foreach(var treePhase in treePhases) {
                treePhase.state = this;
            }

            timeline = new TimelineScrubber(director);
        }

        void OnDestroy() {
            Instance = null;
        }

        void OnEnable() {
            #if UNITY_EDITOR
            if(Application.isEditor) {
                EditorOnEnable();
                return;
            }
            #endif

            ResetTo(TargetProgress());
        }

        void Update() {
            #if UNITY_EDITOR
            if(Application.isEditor) {
                EditorUpdate();
                return;
            }
            #endif

            if(reasonsToBePaused.Count == 0) {
                var target = Math.Min(timeline.Position + maxStepSize, TargetProgress());
                // If rewinding, treat it like a fast-forward reset, skip animations
                if(target < timeline.Position) {
                    ResetTo(target);
                } else {
                    AdvanceTo(target);
                }
            }
        }

        float TargetProgress() {
            return WinterProgress.Instance.GlobalProgress.TreeConstructionPercentage;
        }

        void ResetTo(float percentage) {
            activePhase?.Exit();
            activePhaseIndex = -1;
            isFastForwarding = true;
            timeline.ResetTimeline();
            foreach(var treePart in treeParts) {
                treePart.ResetPart();
            }
            AdvanceTo(percentage);
            isFastForwarding = false;
        }

        void AdvanceTo(float percentage) {
            timeline.SetPercentComplete(percentage);
            while(nextPhase != null && nextPhase.StartAt <= percentage) {
                Debug.Log("advancing to next phase");
                if(activePhase != null) {
                    activePhase.Progress = 1;
                    activePhase.Exit();
                }
                activePhaseIndex++;
                activePhase.Enter();
            }
            Debug.Log("done checking for next phase");
            if(activePhase != null && nextPhase != null) {
                var phaseProgress = (percentage - activePhase.StartAt) / (nextPhase.StartAt - activePhase.StartAt);
                activePhase.Progress = phaseProgress;
            }
        }

#if UNITY_EDITOR

        [SerializeField]
        public float unityEditorPlayButtonStart = 0;
        [SerializeField]
        public float unityEditorPlayButtonEnd = 0;

        private bool unityEditorDidFirstUpdate = false;

        void EditorOnValidate() {
            if(unityEditorPlayButtonStart < MinTime) {
                unityEditorPlayButtonStart = MinTime;
            }
            if(unityEditorPlayButtonEnd < MinTime) {
                unityEditorPlayButtonEnd = MinTime;
            }
        }

        void EditorOnEnable() {
            isFastForwarding = true;
            timeline.SetPercentComplete(unityEditorPlayButtonStart);
            isFastForwarding = false;
        }

        void EditorUpdate() {
            if(!unityEditorDidFirstUpdate) {
                unityEditorDidFirstUpdate = true;
                timeline.SetPercentComplete(unityEditorPlayButtonEnd);
            }
        }

#endif

    }
}
