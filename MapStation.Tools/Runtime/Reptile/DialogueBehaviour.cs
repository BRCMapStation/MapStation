using System;
using UnityEngine;
using UnityEngine.Playables;
using static Reptile.NPC;

namespace Reptile {
    // Token: 0x0200007A RID: 122
    [Serializable]
    public class DialogueBehaviour : PlayableBehaviour {

        // Token: 0x0400038E RID: 910
        [SerializeField]
        private string localizationKey = string.Empty;

        // Token: 0x0400038F RID: 911
        [Header("General Settings")]
        [Tooltip("Set to true if you do not want the timeline to pause once the dialogue line is shown")]
        public bool doNotPause;

        // Token: 0x04000390 RID: 912
        [Tooltip("shows ui bars when dialogue starts. always means the bars stay after dialogue line ends")]
        public DialogueBehaviour.ShowBarsType showBars = DialogueBehaviour.ShowBarsType.OnStartOnly;

        // Token: 0x04000391 RID: 913
        public int speakingCharacterName;

        // Token: 0x04000392 RID: 914
        [Tooltip("Show yes no prompt to user")]
        public DialogueBehaviour.DialogueType dialogueType;

        // Token: 0x04000393 RID: 915
        [Header("Yesno Settings")]
        public bool onlyShowOnYesAnswer;

        // Token: 0x04000394 RID: 916
        public bool onlyShowOnNoAnswer;

        // Token: 0x04000395 RID: 917
        [Header("Audio Settings")]
        [SerializeField]
        public AudioSourceID channel = AudioSourceID.Voices;

        // Token: 0x04000396 RID: 918
        [SerializeField]
        internal SfxClip sfxClip;

        // Token: 0x04000397 RID: 919
        [SerializeField]
        [Range(-1f, 24f)]
        public int indexOfAudioClip = -1;

        // Token: 0x04000398 RID: 920
        [SerializeField]
        public bool randomAudioClip;

        // Token: 0x04000399 RID: 921
        [HideInInspector]
        internal AudioSource audioSource;

        // Token: 0x0400039B RID: 923
        private bool clipPlaying;

        // Token: 0x0400039C RID: 924
        internal PlayableDirector director;

        // Token: 0x020003B1 RID: 945
        public enum DialogueType {
            // Token: 0x04001F18 RID: 7960
            NORMAL_LINE,
            // Token: 0x04001F19 RID: 7961
            YES_NO_GENERIC,
            // Token: 0x04001F1A RID: 7962
            YES_NO_FOR_INVOKE_EVENT,
            // Token: 0x04001F1B RID: 7963
            YES_NO_INVOKE_OR_RESTART_ENCOUNTER
        }

        // Token: 0x020003B2 RID: 946
        public enum ShowBarsType {
            // Token: 0x04001F1D RID: 7965
            NoBars,
            // Token: 0x04001F1E RID: 7966
            OnStartOnly,
            // Token: 0x04001F1F RID: 7967
            Always
        }
    }
}
