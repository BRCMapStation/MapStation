using System;
using UnityEngine;

namespace Reptile {
    // Token: 0x020001EF RID: 495
    public struct MoveAlongPoints_Hot {
        // Token: 0x04000F43 RID: 3907
        public float acc;

        // Token: 0x04000F44 RID: 3908
        public float decc;

        // Token: 0x04000F45 RID: 3909
        public float collisionExtents;

        // Token: 0x04000F46 RID: 3910
        public float speed;

        // Token: 0x04000F47 RID: 3911
        public Transform path;

        // Token: 0x04000F48 RID: 3912
        public float waitAtEnd;

        // Token: 0x04000F49 RID: 3913
        public float waitAtStart;

        // Token: 0x04000F4A RID: 3914
        public bool doNotRotate;

        // Token: 0x04000F4B RID: 3915
        public bool popBack;

        // Token: 0x04000F4C RID: 3916
        public int turnSpeed;

        // Token: 0x04000F4D RID: 3917
        internal Transform tf;

        // Token: 0x04000F4E RID: 3918
        internal bool turnedOff;

        // Token: 0x04000F4F RID: 3919
        internal Vector3 lastPos;

        // Token: 0x04000F51 RID: 3921
        internal bool activelyMoving;

        // Token: 0x04000F52 RID: 3922
        internal bool hasBody;

        // Token: 0x04000F53 RID: 3923
        internal float timer;

        // Token: 0x04000F54 RID: 3924
        internal bool checkNotToHitOthersOnThePath;

        // Token: 0x04000F55 RID: 3925
        internal float stopTimer;

        // Token: 0x04000F56 RID: 3926
        internal int posCount;

        // Token: 0x04000F57 RID: 3927
        internal float waitAtStartTimer;

        // Token: 0x04000F58 RID: 3928
        internal bool stoppedForPlayer;

        // Token: 0x04000F59 RID: 3929
        internal float timeSpeed;

        // Token: 0x04000F5A RID: 3930
        internal int moverInFront;

        // Token: 0x04000F5B RID: 3931
        internal float fracJourney;

        // Token: 0x04000F5C RID: 3932
        internal float leftOver;

        // Token: 0x04000F5D RID: 3933
        internal float journeyLength;

        // Token: 0x04000F5E RID: 3934
        internal float waitAtEndTimer;

        // Token: 0x04000F5F RID: 3935
        internal Rigidbody body;

        // Token: 0x04000F60 RID: 3936
        internal int lastPoint;

        // Token: 0x04000F61 RID: 3937
        internal Vector3 nextPointPos;

        // Token: 0x04000F62 RID: 3938
        internal Vector3 lastPointPos;

        // Token: 0x04000F63 RID: 3939
        internal GameObject gameObject;

        // Token: 0x04000F64 RID: 3940
        internal Vector3[] pathPositions;
    }
}
