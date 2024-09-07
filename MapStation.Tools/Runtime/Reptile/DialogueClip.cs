using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Reptile {
    // Token: 0x0200007B RID: 123
    [Serializable]
    public class DialogueClip : PlayableAsset, ITimelineClipAsset {
        // Token: 0x1700024A RID: 586
        // (get) Token: 0x060005AA RID: 1450 RVA: 0x000142F0 File Offset: 0x000124F0
        public ClipCaps clipCaps {
            get {
                return ClipCaps.None;
            }
        }

        // Token: 0x060005AB RID: 1451 RVA: 0x000142F3 File Offset: 0x000124F3
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            this.template.audioSource = this.exposedAudioSource.Resolve(graph.GetResolver());
            return ScriptPlayable<DialogueBehaviour>.Create(graph, this.template, 0);
        }

        // Token: 0x0400039D RID: 925
        public ExposedReference<AudioSource> exposedAudioSource;

        // Token: 0x0400039E RID: 926
        public DialogueBehaviour template = new DialogueBehaviour();
    }
}
