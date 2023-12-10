using System.IO;
using UnityEngine;
using UnityEngine.Playables;

namespace Winterland.Common {
    /// <summary>
    /// Exists only on the Square stage, connects to the TreeManager and drives the tree's animations, visuals, behavior.
    /// </summary>
    public class TreeController : MonoBehaviour {
        [SerializeField]
        private PlayableDirector director = null;

        private const float MinTime = 0.1f;
        private const float TimelineLength = 10;

        private float timelinePosition;

        void OnEnable() {
            #if UNITY_EDITOR
            if(Application.isEditor) {
                EditorOnEnable();
                return;
            }
            #endif

            timelinePosition = TargetTimelinePosition();
            initializeTimelineToPosition(timelinePosition);
        }

        void Update() {
            #if UNITY_EDITOR
            if(Application.isEditor) {
                EditorUpdate();
                return;
            }
            #endif

            var target = TargetTimelinePosition();
            if(target < timelinePosition) {
                // Rewinding might break animations, so fully reset from the beginning.
                initializeTimelineToPosition(target);
            } else if(target > timelinePosition) {
                advanceTimelineToPosition(target);
            }
            // advanceTimelineToPosition(target);
            timelinePosition = target;
        }

        void initializeTimelineToPosition(float position) {
            director.Stop();
            director.Play();
            director.playableGraph.Evaluate();
            director.playableGraph.Evaluate(position);
        }

        void advanceTimelineToPosition(float position) {
            var deltaTime = position - (float)director.time;
            director.playableGraph.Evaluate(deltaTime);
            Debug.Log($"director time {director.time}");
        }

        float TargetTimelinePosition() {
            return WinterProgress.Instance.GlobalProgress.TreeConstructionPercentage * TimelineLength;
        }

        #if UNITY_EDITOR

        [SerializeField]
        public float unityEditorPlayButtonStart = 0;
        [SerializeField]
        public float unityEditorPlayButtonEnd = 0;

        private bool unityEditorDidFirstUpdate = false;

        void OnValidate() {
            if(director == null) director = GetComponent<PlayableDirector>();
            if(unityEditorPlayButtonStart < MinTime) {
                unityEditorPlayButtonStart = MinTime;
            }
            if(unityEditorPlayButtonEnd < MinTime) {
                unityEditorPlayButtonEnd = MinTime;
            }
        }

        void EditorOnEnable() {
            initializeTimelineToPosition(unityEditorPlayButtonStart);
        }

        void EditorUpdate() {
            if(!unityEditorDidFirstUpdate) {
                unityEditorDidFirstUpdate = true;
                advanceTimelineToPosition(unityEditorPlayButtonEnd);
            }
        }

        #endif

    }
}
