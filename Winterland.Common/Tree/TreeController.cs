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
        public PlayableDirector overallProgressDirector = null;
        [SerializeField]
        public PlayableDirector currentPhaseProgressDirector = null;

        [SerializeField]
        public TreePart[] treeParts;
        [SerializeField]
        public TreePhase[] treePhases;

        public TreeProgress CurrentProgress = new ();
        public TreeProgress TargetProgress { get; set; }

        /// <summary>
        /// Set to true after tree has received starting position and reset itself to that position.
        /// Necessary because there may be a delay before first tree position is received from network.
        /// </summary>
        private bool InitializedTreeProgress = false;

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

        private TimelineScrubber overallProgressTimeline = null;
        private TimelineScrubber phaseProgressTimeline = null;
        
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
                treePhase.Tree = this;
                treePhase.Init();
            }

            if(overallProgressDirector != null) overallProgressTimeline = new TimelineScrubber(overallProgressDirector);
            if(currentPhaseProgressDirector != null) phaseProgressTimeline = new TimelineScrubber(currentPhaseProgressDirector);
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
        }

        void OnDisable() {
            UpdateGlobalProgressBinding();
        }

        // Move entire tree logic into fixed update, because tree parts are physics parts and must animatephysics!
        // Also we must avoid visual pops where a part activates but the appearance animator does not immediately move
        // it beneath the earth (interpolation for example will render one frame at halfway down)
        void FixedUpdate() {
            #if UNITY_EDITOR
            if(Application.isEditor) {
                EditorUpdate();
                return;
            }
            #endif

            if (this.TargetProgress != null) {
                if (this.InitializedTreeProgress) {
                    this.UpdateTreePosition();
                } else {
                    this.InitializeTreePosition();
                }
            }
        }
        
        // Very first update of the tree, resets to starting position
        void InitializeTreePosition() {
            ResetTo(TargetProgress);
            this.InitializedTreeProgress = true;
        }
        
        // All subsequent updates of the treea, animtes forward or resets backward
        void UpdateTreePosition() {
            if(!this.IsConstructionBlocked) {
                // If rewinding, treat it like a fast-forward reset, skip animations
                if(this.CurrentProgress.IsPrecededBy(TargetProgress)) {
                    ResetTo(this.TargetProgress);
                } else {
                    AdvanceToward(this.TargetProgress);
                }
            }
        }

        public void ResetToTarget() {
            ResetTo(this.TargetProgress);
        }

        private void ResetTo(TreeProgress progress) {
            isFastForwarding = true;
            // Reset everything to beginning
            this.CurrentProgress.ActivePhaseIndex = -1;
            this.CurrentProgress.ActivePhaseProgress = 0;
            overallProgressTimeline?.ResetTimeline();
            phaseProgressTimeline?.ResetTimeline();
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

        private void AdvanceToward(TreeProgress target) {
            // Responsibilities:
            // Update CurrentProgress
            // call methods on Phases so they're accurate
            // Halt partway if animation wants us to pause, but *not* if we're resetting/fast-forwarding

            bool blocked() {
                return !this.isFastForwarding && this.IsConstructionBlocked;
            }

            while (true) {
                // If anything we've done has caused a construction block (we started an animation?)
                // then stop here, wait for the block to be lifted.
                if(blocked()) break;
                
                // Should we start the current phase?
                if (ActivePhase != null && ActivePhase.State == TreePhase.TreePhaseState.Pending) {
                    ActivePhase.Progress = 0;
                    ActivePhase.Enter();
                    continue;
                }

                // If haven't reached target phase yet
                if (target.ActivePhaseIndex > CurrentProgress.ActivePhaseIndex) {
                    
                    // Should we end current phase?
                    if (ActivePhase != null && ActivePhase.State == TreePhase.TreePhaseState.Active) {
                        ActivePhase.Progress = 1;
                        ActivePhase.Exit();
                        continue;
                    }

                    // Should we advance to next phase?  Once current phase is finished
                    if (NextPhase != null &&
                        (ActivePhase == null || ActivePhase.State == TreePhase.TreePhaseState.Finished)) {
                        CurrentProgress.ActivePhaseIndex++;
                        CurrentProgress.ActivePhaseProgress = 0;
                        continue;
                    }
                }

                // Have we reached target phase? If so, set its progress percentage
                if (CurrentProgress.ActivePhaseIndex == target.ActivePhaseIndex && ActivePhase != null) {
                    CurrentProgress.ActivePhaseProgress = target.ActivePhaseProgress;
                    ActivePhase.Progress = target.ActivePhaseProgress;
                    break;
                }
                
                // If we get here, no actions are possible.
                // Do not endless loop.
                break;
            }
            
            // At this point, CurrentProgress matches current state of the phases above.
            // Set overall timeline to match.
            if(overallProgressTimeline != null) overallProgressTimeline.SetPercentComplete(OverallProgress(CurrentProgress));
            if(phaseProgressTimeline != null) phaseProgressTimeline.SetPercentComplete(CurrentProgress.ActivePhaseProgress);
        }

        // Given we have X phases total, and are in phase Y at Z% complete, what % complete are we overall?
        // Not scientific, just to drive timelines
        private float OverallProgress(TreeProgress progress) {
            return (progress.ActivePhaseIndex + progress.ActivePhaseProgress) / this.treePhases.Length;
        }
        
        // Bind or unbind to changes in IGlobalProgress
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

        /// <summary>
        /// Derive target tree progress from global server-synced event progress,
        /// return null if we haven't received server-synced progress yet.
        /// </summary>
        public static TreeProgress TreeProgressFromGlobalProgress() {
            // First phase that's not 100% completed is the active phase.
            // NOTE TO SELF (cspotcode)
            // Server and tree can have different concepts of "active phase" if
            // server has ActivateNextPhaseAutomatically disabled.
            // For example, if server shows phase 0 = active with 27/25 gifts collected,
            // tree will interpret as phase 0 completed, so phase 1 active, even though
            // server is still counting gifts collected towards phase 0.
            var state = WinterProgress.Instance.GlobalProgress.State;
            if(state == null) return null;
            for(var i = 0; i < state.Phases.Count; i++) {
                var phase = state.Phases[i];
                if (phase.GiftsCollected < phase.GiftsGoal) {
                    float progress = 0;
                    if (phase.GiftsCollected >= phase.GiftsGoal) progress = 1f;
                    else if(phase.GiftsGoal > 0) progress = ((float) phase.GiftsCollected) / phase.GiftsGoal;
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
