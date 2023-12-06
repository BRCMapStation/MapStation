using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    public abstract class SequenceAction : MonoBehaviour {
        public CameraRegisterer Camera;
        [HideInInspector]
        public CustomNPC NPC;
        [HideInInspector]
        public SequenceWrapper Sequence;
        [HideInInspector]
        public SequenceAction NextAction;
        public virtual void Run() {
            if (Camera != null)
                Sequence.SetCamera(Camera.gameObject);
        }

        protected void Finish() {
            if (NextAction == null)
                return;
            NextAction.Run();
        }
    }
}
