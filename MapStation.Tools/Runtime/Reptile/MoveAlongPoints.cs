using System;
using UnityEngine;

namespace Reptile {
    // Token: 0x020001ED RID: 493
    public class MoveAlongPoints : MonoBehaviour {
        // Token: 0x04000F2E RID: 3886
        private BaseMoveHandler handler;

        // Token: 0x04000F2F RID: 3887
        public float acc = 1f;

        // Token: 0x04000F30 RID: 3888
        public float decc = 1f;

        // Token: 0x04000F31 RID: 3889
        public float collisionExtents;

        // Token: 0x04000F32 RID: 3890
        public float speed = 1f;

        // Token: 0x04000F33 RID: 3891
        public Transform path;

        // Token: 0x04000F34 RID: 3892
        public float waitAtEnd;

        // Token: 0x04000F35 RID: 3893
        public float waitAtStart;

        // Token: 0x04000F36 RID: 3894
        public bool doNotRotate;

        // Token: 0x04000F37 RID: 3895
        public bool popBack;

        // Token: 0x04000F38 RID: 3896
        public int turnSpeed = 3;

        // Token: 0x04000F39 RID: 3897
        public float delay;

        // Token: 0x04000F3A RID: 3898
        public float delayMultiplier = 1f;

        // Token: 0x04000F3B RID: 3899
        public bool stopForPlayer;

        // Token: 0x04000F3C RID: 3900
        public bool stopForStreetLife;

        // Token: 0x04000F3D RID: 3901
        public bool stopForEnemies;

        // Token: 0x04000F3E RID: 3902
        public bool startOnRandomPoint;

        // Token: 0x04000F3F RID: 3903
        internal bool partOfCluster;
    }
}
