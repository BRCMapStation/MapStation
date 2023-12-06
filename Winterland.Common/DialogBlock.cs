using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public class DialogBlock : MonoBehaviour {
        [HideInInspector]
        public DialogSequenceAction Owner;
        public enum SpeakerMode {
            None,
            NPC,
            Text
        }
        public SpeakerMode Mode = SpeakerMode.NPC;
        public string SpeakerName = "???";
        public string Text = "...";
    }
}
