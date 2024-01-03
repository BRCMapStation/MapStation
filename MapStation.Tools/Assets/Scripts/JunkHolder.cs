using System;
using UnityEngine;
using MapStation.Common;

namespace Reptile {
    [RequireComponent(typeof(StageObject))]
    [ExecuteAlways]
    public class JunkHolder : MonoBehaviour {
        // Token: 0x04000E8B RID: 3723
        internal bool moved;

        // Token: 0x04000E8C RID: 3724
        private Junk[] junkChildren;

        // Token: 0x04000E8D RID: 3725
        public Rigidbody[] stuffToLaunchAway;

        // Token: 0x04000E8E RID: 3726
        public SfxClip audioClips = SfxCollectionID.EnvironmentSfx;

        private void Reset() {
            gameObject.layer = 21;
        }
    }
}
