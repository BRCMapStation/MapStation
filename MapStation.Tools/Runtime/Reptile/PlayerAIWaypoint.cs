using System;
using UnityEngine;

namespace Reptile {
    // Token: 0x0200027C RID: 636
    public class PlayerAIWaypoint : MonoBehaviour {

        // Token: 0x04001549 RID: 5449
        public bool boost;

        // Token: 0x0400154A RID: 5450
        public bool skipIfCantReach;

        // Token: 0x0400154B RID: 5451
        public bool walk;

        // Token: 0x0400154C RID: 5452
        public bool unequipMovestyle;

        // Token: 0x0400154D RID: 5453
        public bool slide;

        // Token: 0x0400154E RID: 5454
        [Header("trick input can interfere with jumps from grinds")]
        public bool trick;

        // Token: 0x0400154F RID: 5455
        public float wait;

        // Token: 0x04001550 RID: 5456
        public float grindTiltCorner;

        // Token: 0x04001551 RID: 5457
        [Header("if checked the AI gets teleported to the next waypoint")]
        public bool autoTeleport;

        // Token: 0x04001552 RID: 5458
        public bool dontCheatTeleportTo;

        // Token: 0x04001553 RID: 5459
        public bool ignoreObstructions;

        // Token: 0x04001555 RID: 5461
        public PlayerAIWaypoint.JumpPointBehavoir jumpPointBehavoir;

        // Token: 0x020004D0 RID: 1232
        public enum JumpPointBehavoir {
            // Token: 0x04002814 RID: 10260
            REACH_WITH_AIRDASH_IF_FAR,
            // Token: 0x04002815 RID: 10261
            AIRDASH_AT_JUMP_POINT,
            // Token: 0x04002816 RID: 10262
            TRICK_AT_JUMP_POINT,
            // Token: 0x04002817 RID: 10263
            JUST_MOVE_TO_JUMP_POINT
        }
    }
}
