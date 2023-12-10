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

        [SerializeField]
        public float unityEditorPlayButtonStart = 0;
        [SerializeField]
        public float unityEditorPlayButtonEnd = 0;

        private bool didFirstUpdate = false;
        private const float MinTime = 0.1f;

        void OnValidate() {
            if(director == null) director = GetComponent<PlayableDirector>();
            if(unityEditorPlayButtonStart < MinTime) {
                unityEditorPlayButtonStart = MinTime;
            }
            if(unityEditorPlayButtonEnd < MinTime) {
                unityEditorPlayButtonEnd = MinTime;
            }
        }

        void OnEnable() {
            if(Application.isEditor) {
                EditorOnEnable();
                return;
            }
        }

        void Update() {
            if(Application.isEditor) {
                EditorUpdate();
                return;
            }
        }

        void EditorOnEnable() {
            director.Play();
            director.playableGraph.Evaluate();
            director.playableGraph.Evaluate(unityEditorPlayButtonStart);
        }

        void EditorUpdate() {
            var deltaTime = 0f;
            if(!didFirstUpdate) {
                deltaTime = unityEditorPlayButtonEnd - (float)director.time;
                didFirstUpdate = true;
            }
            director.playableGraph.Evaluate(deltaTime);
            Debug.Log($"director time {director.time}");
        }
    }
}
