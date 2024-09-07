using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Reptile {
    // Token: 0x02000078 RID: 120
    [Serializable]
    public class BlackBarClip : PlayableAsset, ITimelineClipAsset {
        // Token: 0x17000249 RID: 585
        // (get) Token: 0x0600059F RID: 1439 RVA: 0x00014109 File Offset: 0x00012309
        public ClipCaps clipCaps {
            get {
                return ClipCaps.None;
            }
        }

        // Token: 0x060005A0 RID: 1440 RVA: 0x0001410C File Offset: 0x0001230C
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            return ScriptPlayable<BlackBarBehaviour>.Create(graph, this.template, 0);
        }

        // Token: 0x0400038D RID: 909
        public BlackBarBehaviour template = new BlackBarBehaviour();
    }
}
