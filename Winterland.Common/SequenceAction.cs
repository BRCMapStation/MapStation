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
        // Unused ATM but should be set to true on actions that only make sense during the cutscene, not when skipped, such as dialogues and such. This is for actions that can run on cutscene start and end.
        public virtual bool SequenceOnly => false;
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

        public virtual void Run() {
            
        }

        protected void Finish() {
            if (NextAction == null)
                Sequence.ExitSequence();
            else
                NextAction.Run();
        }
    }
}
