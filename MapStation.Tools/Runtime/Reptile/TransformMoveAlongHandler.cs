using System;
using System.Collections.Generic;
using UnityEngine;

namespace Reptile {
    // Token: 0x020001FB RID: 507
    public abstract class TransformMoveAlongHandler : BaseMoveHandler {
        // Token: 0x04000FB7 RID: 4023
        protected List<MoveAlongPoints_Hot> moversHot;

        // Token: 0x04000FB8 RID: 4024
        protected List<MoveAlongPoints> moversCold;
    }
}
