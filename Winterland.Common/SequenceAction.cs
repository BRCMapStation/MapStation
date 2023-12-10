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
    public abstract class SequenceAction : OrderedComponent {
        [Header("Name used to point to this action when branching. Used in Yes/Nah prompts.")]
        public string Name = "";
        [HideInInspector]
        public CustomNPC NPC;
        [HideInInspector]
        public SequenceWrapper Sequence;
        [HideInInspector]
        public SequenceAction NextAction;

        protected override void OnValidate() {
            base.OnValidate();
            if (!Application.isEditor)
                return;
            hideFlags = HideFlags.HideInInspector;
            var sequence = gameObject.GetComponent<Sequence>();
            if (sequence == null)
                Destroy(this);
        }

        public override bool IsPeer(Component other) {
            return other is SequenceAction;
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
