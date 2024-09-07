using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Reptile {
    // Token: 0x0200008F RID: 143
    [Serializable]
    public class StyleSwitchClip : PlayableAsset, ITimelineClipAsset {
        // Token: 0x17000256 RID: 598
        // (get) Token: 0x06000607 RID: 1543 RVA: 0x000158B3 File Offset: 0x00013AB3
        public ClipCaps clipCaps {
            get {
                return ClipCaps.None;
            }
        }

        // Token: 0x06000608 RID: 1544 RVA: 0x000158B6 File Offset: 0x00013AB6
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            return ScriptPlayable<StyleSwitchBehaviour>.Create(graph, this.settings, 0);
        }

        // Token: 0x040003E6 RID: 998
        public StyleSwitchBehaviour settings = new StyleSwitchBehaviour();
    }
}
