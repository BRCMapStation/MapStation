using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapStation.Common;

namespace Reptile {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(StageObject))]

    [ExecuteAlways]
    public class Junk : MonoBehaviour {

        private void Reset() {
            gameObject.layer = 21;
        }

        // Token: 0x04000E72 RID: 3698
        public Junk.Interact interactOn = Junk.Interact.ON_RUN_IN_AND_HITBOX;

        // Token: 0x04000E73 RID: 3699
        public float destroyTimer = -1f;

        // Token: 0x04000E74 RID: 3700
        public SfxClip audioClips = SfxCollectionID.EnvironmentSfx;

        // Token: 0x04000E75 RID: 3701
        internal JunkBehaviour junkBehaviour;

        // Token: 0x04000E76 RID: 3702
        internal JunkHolder junkHolder;

        // Token: 0x04000E77 RID: 3703
        internal Rigidbody rigidBody;

        // Token: 0x04000E78 RID: 3704
        internal Quaternion originRotation;

        // Token: 0x04000E79 RID: 3705
        internal Vector3 originPosition;

        // Token: 0x04000E7A RID: 3706
        internal Vector3 hitPosition;

        // Token: 0x04000E7B RID: 3707
        internal Vector3 hitForce;

        // Token: 0x04000E7C RID: 3708
        internal float rehitTimer;

        // Token: 0x04000E7D RID: 3709
        internal float hitPauseTimer;

        // Token: 0x04000E7E RID: 3710
        internal float hitpauseDuration = 0.04f;

        // Token: 0x04000E7F RID: 3711
        internal int deactiveCounter;

        // Token: 0x04000E80 RID: 3712
        internal const float DEFAULT_KICK_DIRECTION_FORCE = 20f;

        // Token: 0x04000E81 RID: 3713
        internal const float SPECIAL_KICK_DIRECTION_FORCE = 4f;

        // Token: 0x04000E82 RID: 3714
        internal const float DEFAULT_KICK_UP_FORCE = 5f;

        // Token: 0x04000E83 RID: 3715
        internal const float SPECIAL_KICK_UP_FORCE = 11f;

        // Token: 0x04000E84 RID: 3716
        internal int _index;

        // Token: 0x04000E85 RID: 3717
        internal int _startIndex;

        // Token: 0x0200040D RID: 1037
        public enum Interact {
            // Token: 0x040021CB RID: 8651
            ON_HITBOX,
            // Token: 0x040021CC RID: 8652
            ON_RUN_IN_AND_HITBOX,
            // Token: 0x040021CD RID: 8653
            ON_AWAKE,
            // Token: 0x040021CE RID: 8654
            ON_TOUCH
        }
    }
}
