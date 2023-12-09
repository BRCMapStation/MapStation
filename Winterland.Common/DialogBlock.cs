using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    [ExecuteAlways]
    public class DialogBlock : OrderedComponent {
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
        [Header("Random clips to play when the character says this line.")]
        public AudioClip[] AudioClips;

        private void Awake() {
            if (!Application.isEditor)
                return;
            hideFlags = HideFlags.HideInInspector;
            var dialogSequenceAction = GetComponent<DialogSequenceAction>();
            if (dialogSequenceAction == null)
                DestroyImmediate(this);
        }

        public override bool IsPeer(Component other) {
            var otherBlock = other as DialogBlock;
            if (otherBlock == null)
                return false;
            if (otherBlock.Owner == Owner)
                return true;
            return false;
        }

        private void Start() {
            if (!Application.isEditor)
                return;
            if (Owner == null)
                DestroyImmediate(this);
        }
    }
}
