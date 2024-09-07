using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Reptile {
    // Token: 0x02000077 RID: 119
    [Serializable]
    public class BlackBarBehaviour : PlayableBehaviour {
        // Token: 0x0600059C RID: 1436 RVA: 0x00014060 File Offset: 0x00012260
        public override void OnBehaviourPlay(Playable playable, FrameData info) {
        }

        // Token: 0x0600059D RID: 1437 RVA: 0x000140A4 File Offset: 0x000122A4
        public override void OnBehaviourPause(Playable playable, FrameData info) {
        }

        // Token: 0x0400038A RID: 906
        [Tooltip("Height of black bars.")]
        public float barHeight = 150f;

        // Token: 0x0400038B RID: 907
        [Tooltip("Duration of fade in")]
        public float fadeInTime = 0.25f;

        // Token: 0x0400038C RID: 908
        [Tooltip("Duration of fade out")]
        public float fadeOutTime = 0.25f;
    }
}
