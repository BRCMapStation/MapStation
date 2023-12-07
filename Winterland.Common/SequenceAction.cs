using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using Reptile;

namespace Winterland.Common {

    [ExecuteAlways]
    public abstract class SequenceAction : MonoBehaviour {
        [Header("Name used to point to this action when branching. Used in Yes/Nah prompts.")]
        public string Name = "";
        [Header("Leave this on None to keep the current camera.")]
        public CameraRegisterer Camera;
        [HideInInspector]
        public CustomNPC NPC;
        [HideInInspector]
        public SequenceWrapper Sequence;
        [HideInInspector]
        public SequenceAction NextAction;

        private void Awake() {
            if (!Application.isEditor)
                return;
            hideFlags = HideFlags.HideInInspector;
            var sequence = GetComponent<Sequence>();
            if (sequence == null)
                DestroyImmediate(this);
        }

        public virtual void Run() {
            if (Camera != null)
                Sequence.SetCamera(Camera.gameObject);
        }

        protected void Finish() {
            if (NextAction == null)
                Sequence.ExitSequence();
            else
                NextAction.Run();
        }

        protected SequenceAction GetActionByName(string name) {
            if (string.IsNullOrEmpty(name))
                return null;
            var actions = Sequence.Sequence.GetActions();
            foreach(var action in actions) {
                if (action.Name == name)
                    return action;
            }
            return null;
        }
    }
}
