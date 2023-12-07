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

        public virtual void Run(bool immediate) {
            
        }

        protected void Finish(bool immediate) {
            if (NextAction == null) {
                if (immediate)
                    return;
                Sequence.ExitSequence();
            } else {
                NextAction.Run(immediate);
            }
        }
    }
}
