using System;
using UnityEngine;

namespace Reptile {
    // Token: 0x020001D2 RID: 466
    public class JunkHolder : MonoBehaviour {
        // Token: 0x04000E8B RID: 3723
        internal bool moved;

        // Token: 0x04000E8C RID: 3724
        private Junk[] junkChildren;

        // Token: 0x04000E8D RID: 3725
        public Rigidbody[] stuffToLaunchAway;

        // Token: 0x04000E8E RID: 3726
        public SfxClip audioClips = SfxCollectionID.EnvironmentSfx;
    }
}
