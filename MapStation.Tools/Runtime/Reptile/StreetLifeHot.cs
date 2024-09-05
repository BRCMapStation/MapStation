using System;
using UnityEngine;

namespace Reptile {
    // Token: 0x02000203 RID: 515
    public struct StreetLifeHot {
        // Token: 0x04000FD6 RID: 4054
        public int idleAnimation;

        // Token: 0x04000FD7 RID: 4055
        public int moveAwayAnimation;

        // Token: 0x04000FD8 RID: 4056
        public int walkAnimation;

        // Token: 0x04000FD9 RID: 4057
        public Junk attachedJunk;

        // Token: 0x04000FDA RID: 4058
        internal bool isShook;

        // Token: 0x04000FDB RID: 4059
        internal bool wasShook;

        // Token: 0x04000FDC RID: 4060
        internal float moveTimer;

        // Token: 0x04000FDD RID: 4061
        internal float moveAwayDuration;

        // Token: 0x04000FDE RID: 4062
        internal float moveAwaySpeed;

        // Token: 0x04000FDF RID: 4063
        internal bool isGrounded;

        // Token: 0x04000FE0 RID: 4064
        internal GameObject activationGameObject;

        // Token: 0x04000FE1 RID: 4065
        internal Transform transform;

        // Token: 0x04000FE2 RID: 4066
        internal Collider trigger;

        // Token: 0x04000FE3 RID: 4067
        internal Vector3 verticalDir;

        // Token: 0x04000FE4 RID: 4068
        internal float verticalSpeed;

        // Token: 0x04000FE5 RID: 4069
        internal float verticalAcc;

        // Token: 0x04000FE6 RID: 4070
        internal float maxVerticalSpeed;

        // Token: 0x04000FE7 RID: 4071
        internal float curSpeed;

        // Token: 0x04000FE8 RID: 4072
        internal bool collided;

        // Token: 0x04000FE9 RID: 4073
        internal Vector3 moveDir;

        // Token: 0x04000FEA RID: 4074
        internal Animator anim;

        // Token: 0x04000FEB RID: 4075
        internal Vector3 startPos;

        // Token: 0x04000FEC RID: 4076
        internal Quaternion startRot;

        // Token: 0x04000FED RID: 4077
        internal MoveAlongPoints moveAlongPoints;

        // Token: 0x04000FEE RID: 4078
        internal int currentAnim;
    }
}
