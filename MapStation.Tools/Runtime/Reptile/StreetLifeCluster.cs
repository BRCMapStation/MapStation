using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Reptile {
    public class StreetLifeCluster : TransformMoveAlongHandler {
        // Token: 0x04000FF1 RID: 4081
        public int hideFromWantedStar = 2;

        // Token: 0x04000FF2 RID: 4082
        internal bool offDueToWantedStatus;

        // Token: 0x04000FF3 RID: 4083
        private StreetLifeBehaviour streetLifeBehaviour;

        // Token: 0x04000FF4 RID: 4084
        private BirdBehaviour birdBehaviour;

        // Token: 0x04000FF5 RID: 4085
        private StreetLifeHot[] streetLife;

        // Token: 0x04000FF6 RID: 4086
        private StreetLife[] pedestrians;

        // Token: 0x04000FF7 RID: 4087
        private Bird[] birds;

        // Token: 0x04000FF8 RID: 4088
        private int birdsStartIndex;

        // Token: 0x04000FF9 RID: 4089
        private int distanceCheckIterator;

        // Token: 0x04000FFA RID: 4090
        public bool distanceOptimize = true;

        // Token: 0x04000FFB RID: 4091
        private Transform tf;

        // Token: 0x04000FFC RID: 4092
        private Vector3 clusterPosition = Vector3.zero;

        // Token: 0x04000FFD RID: 4093
        private float deactivateDistance = 63f;

        // Token: 0x04000FFE RID: 4094
        private bool shortDistanceLOD;

        // Token: 0x04000FFF RID: 4095
        private bool clusterActive = true;
    }
}
