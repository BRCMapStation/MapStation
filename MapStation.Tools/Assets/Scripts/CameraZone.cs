using System;
using UnityEngine;

namespace Reptile {
    // Token: 0x020001D9 RID: 473
    public class CameraZone : MonoBehaviour {
        // Token: 0x04000EBC RID: 3772
        public Transform fixedCameraPoint;

        // Token: 0x04000EBD RID: 3773
        public string showAreaName = string.Empty;

        // Token: 0x04000EBE RID: 3774
        public string localizationKey = string.Empty;

        // Token: 0x04000EBF RID: 3775
        public bool attachedToStageExit;

        // Token: 0x04000EC0 RID: 3776
        public CameraZoneMode cameraMode = CameraZoneMode.FIXED;

        // Token: 0x04000EC1 RID: 3777
        public BoxSide planeLimitedSide;

        // Token: 0x04000EC2 RID: 3778
        public bool cylinderPlaneLimited;

        // Token: 0x04000EC3 RID: 3779
        public bool onlyOnGrinds;

        // Token: 0x04000EC4 RID: 3780
        internal BoxCollider box;

        // Token: 0x04000EC5 RID: 3781
        //private IGameTextLocalizer localizer;

        // Token: 0x04000EC6 RID: 3782
        //private Stage cameraZoneStage = Stage.NONE;

        // Token: 0x04000EC7 RID: 3783
        [Tooltip("For FIXED_ANGLE mode, the camera viewing angle (0 = blue arrow of zone box, positive = turned clockwise)")]
        public float fixedAngle;

        // Token: 0x04000EC8 RID: 3784
        [Tooltip("For FIXED_ANGLE mode, the camera distance from the player")]
        public float fixedAngleDistance;
    }
}
