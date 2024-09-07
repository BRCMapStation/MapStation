using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Reptile {
    // Token: 0x02000089 RID: 137
    [Serializable]
    public class BaseMenuTimelineBehaviour : PlayableBehaviour {
        // Token: 0x040003CA RID: 970
        private const float BLACK_BARS_ANIMATION_DURATION = 0.25f;

        // Token: 0x040003CB RID: 971
        public bool onlyShowOnYesAnswer = true;

        // Token: 0x040003CC RID: 972
        public bool onlyShowOnNoAnswer;

        // Token: 0x040003CD RID: 973
        [Tooltip("Shows ui bars when clip starts. Always means the bars stay after clip ends")]
        public BaseMenuTimelineBehaviour.ShowBarsType showBars = BaseMenuTimelineBehaviour.ShowBarsType.OnStartOnly;

        // Token: 0x040003CE RID: 974
        protected PlayableDirector director;

        // Token: 0x040003CF RID: 975
        private bool pauseScheduled;

        // Token: 0x040003D1 RID: 977
        private bool hasStarted;

        // Token: 0x020003B3 RID: 947
        public enum ShowBarsType {
            // Token: 0x04001F21 RID: 7969
            NoBars,
            // Token: 0x04001F22 RID: 7970
            OnStartOnly,
            // Token: 0x04001F23 RID: 7971
            Always
        }
    }
}
