using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    [ExecuteAlways]
    public class DialogBlock : MonoBehaviour {
        [HideInInspector]
        public DialogSequenceAction Owner;
        public enum SpeakerMode {
            None,
            NPC,
            Text
        }
        public SpeakerMode Speaker = SpeakerMode.NPC;
        [HideInInspector]
        public string SpeakerName = "???";
        public string Text = "...";

        private void Awake() {
            if (!Application.isEditor)
                return;
            hideFlags = HideFlags.HideInInspector;
            var dialogSequenceAction = GetComponent<DialogSequenceAction>();
            if (dialogSequenceAction == null)
                DestroyImmediate(this);
        }

        private void Start() {
            if (!Application.isEditor)
                return;
            if (Owner == null)
                DestroyImmediate(this);
        }
    }
}
