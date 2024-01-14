using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Reptile {
    // Token: 0x020001D1 RID: 465
    public class JunkBehaviour {
        // Token: 0x04000E86 RID: 3718
        private const float tooFarDistanceSquared = 14400f;

        // Token: 0x04000E87 RID: 3719
        internal Junk[] totalJunk;

        // Token: 0x04000E88 RID: 3720
        internal int kickedJunkIndex;

        // Token: 0x04000E89 RID: 3721
        internal int nonupdatingJunkIndex;

        // Token: 0x04000E8A RID: 3722
        internal float junkCheckTimer;
    }
}
