using System;
using System.Collections.Generic;
using Reptile;
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

        [SerializeField]
        public TreePart[] treeParts;
        [SerializeField]
        public TreePhase[] treePhases;

        public TreeProgress CurrentProgress = new ();
        public TreeProgress TargetProgress { get; set; }

        public int ActivePhaseIndex => this.CurrentProgress.ActivePhaseIndex;
        public float ActivePhaseProgress => this.CurrentProgress.ActivePhaseProgress;
        
        public TreePhase ActivePhase => ActivePhaseIndex >= 0 && ActivePhaseIndex < treePhases.Length ? treePhases[ActivePhaseIndex] : null;
        public TreePhase NextPhase => ActivePhaseIndex >= -1 && ActivePhaseIndex < treePhases.Length - 1 ? treePhases[ActivePhaseIndex + 1] : null;

        private bool isFastForwarding;
        public bool IsFastForwarding {
            get => isFastForwarding;
            private set {
                isFastForwarding = value;
                foreach(var treePart in treeParts) {
                    treePart.animator?.SetBoolString("IsFastForwarding", value);
                }
            }
        }
        
        [HideInInspector]
        private HashSet<ITreeConstructionBlocker> constructionBlockers = new ();
        public HashSet<ITreeConstructionBlocker> ConstructionBlockers => this.constructionBlockers;

        private bool IsConstructionBlocked => this.constructionBlockers.Count > 0;

        private TimelineScrubber timeline;
        
        // Set false for manual animation testing
        private bool syncToGlobalProgress = true;
        public bool SyncToGlobalProgress {
            get => syncToGlobalProgress;
            set {
                syncToGlobalProgress = value;
                UpdateGlobalProgressBinding();
            }
        }

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
                treePhase.Init();
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

            UpdateGlobalProgressBinding();

            ResetTo(TargetProgress);
        }

        void OnDisable() {
            UpdateGlobalProgressBinding();
        }

        void Update() {
            #if UNITY_EDITOR
            if(Application.isEditor) {
                EditorUpdate();
                return;
            }
            #endif

            if(!this.IsConstructionBlocked) {
                // If rewinding, treat it like a fast-forward reset, skip animations
                if(this.IsInPast(TargetProgress)) {
                    ResetTo(this.TargetProgress);
                } else {
                    AdvanceToward(this.TargetProgress);
                }
            }
        }

        private void ResetTo(TreeProgress progress) {
            isFastForwarding = true;
            // Reset everything to beginning
            if(this.ActivePhase != null) this.ActivePhase.Exit();
            this.CurrentProgress.ActivePhaseIndex = -1;
            this.CurrentProgress.ActivePhaseProgress = 0;
            timeline.ResetTimeline();
            foreach(var phase in treePhases) {
                phase.ResetPhase();
            }
            foreach(var treePart in treeParts) {
                treePart.ResetPart();
            }
            // Fast-forward to target
            AdvanceToward(progress);
            isFastForwarding = false;
        }

        private void AdvanceToward(TreeProgress progress) {
            timeline.SetPercentComplete(OverallProgress(progress));
            while(this.NextPhase != null && progress.ActivePhaseIndex > this.CurrentProgress.ActivePhaseIndex && !this.IsConstructionBlocked) {
                if(this.ActivePhase != null) {
                    this.ActivePhase.Progress = 1;
                    this.ActivePhase.Exit();
                }
                CurrentProgress.ActivePhaseIndex++;
                this.ActivePhase.Enter();
            }
            if(this.ActivePhaseIndex == progress.ActivePhaseIndex && this.ActivePhase != null) {
                this.ActivePhase.Progress = progress.ActivePhaseProgress;
            }
        }

        // True if we have to rewind to reach the target
        private bool IsInPast(TreeProgress progress) {
            return CurrentProgress.ActivePhaseIndex > progress.ActivePhaseIndex ||
                   CurrentProgress.ActivePhaseProgress > progress.ActivePhaseProgress;
        }

        // Given we have X phases total, and are in phase Y at Z% complete, what % complete are we overall?
        // Not scientific, just to drive timelines
        private float OverallProgress(TreeProgress progress) {
            return (progress.ActivePhaseIndex + progress.ActivePhaseProgress) / this.treePhases.Length;
        }

        private void UpdateGlobalProgressBinding() {
            WinterProgress.Instance.GlobalProgress.OnGlobalStateChanged -= OnGlobalStateChanged;
            if(syncToGlobalProgress && isActiveAndEnabled) {
                WinterProgress.Instance.GlobalProgress.OnGlobalStateChanged += OnGlobalStateChanged;
                TargetProgress = TreeProgressFromGlobalProgress();
            }
        }
        
        private void OnGlobalStateChanged() {
            TargetProgress = TreeController.TreeProgressFromGlobalProgress();
        }

        public static TreeProgress TreeProgressFromGlobalProgress() {
            // First phase that's not 100% completed is the active phase
            var state = WinterProgress.Instance.GlobalProgress.State;
            for(var i = 0; i < state.Phases.Count; i++) {
                var phase = state.Phases[i];
                if (phase.GiftsCollected < phase.GiftsGoal) {
                    float progress = 0;
                    if (phase.GiftsCollected >= phase.GiftsGoal) progress = 1f;
                    else if(phase.GiftsGoal > 0) progress = (float) phase.GiftsCollected / (float) phase.GiftsGoal;
                    return new TreeProgress() {
                        ActivePhaseIndex = i,
                        ActivePhaseProgress = progress 
                    };
                }
            }
            return new TreeProgress();
        }

#if UNITY_EDITOR

        // [HideInInspector]    
        // [SerializeField]
        // public float unityEditorPlayButtonStart = 0;
        // [HideInInspector]    
        // [SerializeField]
        // public float unityEditorPlayButtonEnd = 0;
        //
        // private bool unityEditorDidFirstUpdate = false;

        void EditorOnValidate() {
            // if(unityEditorPlayButtonStart < MinTime) {
            //     unityEditorPlayButtonStart = MinTime;
            // }
            // if(unityEditorPlayButtonEnd < MinTime) {
            //     unityEditorPlayButtonEnd = MinTime;
            // }
        }

        void EditorOnEnable() {
            // isFastForwarding = true;
            // timeline.SetPercentComplete(unityEditorPlayButtonStart);
            // isFastForwarding = false;
        }

        void EditorUpdate() {
            // if(!unityEditorDidFirstUpdate) {
            //     unityEditorDidFirstUpdate = true;
            //     timeline.SetPercentComplete(unityEditorPlayButtonEnd);
            // }
        }

#endif

    }
}
