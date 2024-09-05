using System;
using UnityEngine;

namespace Reptile {
    // Token: 0x02000202 RID: 514
    [RequireComponent(typeof(AudioSource))]
    public class StreetLife : MonoBehaviour {
        // Token: 0x04000FC8 RID: 4040
        internal StreetLifeCluster cluster;

        // Token: 0x04000FC9 RID: 4041
        internal int indexIntoCluster;

        // Token: 0x04000FCA RID: 4042
        public StreetLife.IdleAnimation idleAnimation;

        // Token: 0x04000FCB RID: 4043
        public StreetLife.MoveAwayAnimation moveAwayAnimation;

        // Token: 0x04000FCC RID: 4044
        public StreetLife.WalkAnimation walkAnimation;

        // Token: 0x04000FCD RID: 4045
        public Junk attachedJunk;

        // Token: 0x04000FCE RID: 4046
        internal const SfxCollectionID sfxCollection = SfxCollectionID.StreetlifeSfx;

        // Token: 0x04000FCF RID: 4047
        public bool reportJostleAsCrime;

        // Token: 0x04000FD0 RID: 4048
        public int streetlifeID = -1;

        // Token: 0x04000FD1 RID: 4049
        internal AudioSource audioSource;

        // Token: 0x04000FD2 RID: 4050
        internal SkinnedMeshRenderer skinnedMeshRenderer;

        // Token: 0x04000FD3 RID: 4051
        internal float lastAudioTime;

        // Token: 0x04000FD4 RID: 4052
        internal float lastJostleTime;

        // Token: 0x04000FD5 RID: 4053
        internal bool isInitialized;

        // Token: 0x02000467 RID: 1127
        public enum IdleAnimation {
            // Token: 0x0400226A RID: 8810
            pedIdleConfidence,
            // Token: 0x0400226B RID: 8811
            pedIdleCute,
            // Token: 0x0400226C RID: 8812
            pedIdleSideArm,
            // Token: 0x0400226D RID: 8813
            pedIdleUpset,
            // Token: 0x0400226E RID: 8814
            pedIdleStressed,
            // Token: 0x0400226F RID: 8815
            pedIdleSprayed,
            // Token: 0x04002270 RID: 8816
            pedIdleKnocking,
            // Token: 0x04002271 RID: 8817
            pedIdleSitPhone,
            // Token: 0x04002272 RID: 8818
            pedIdleSitLedge,
            // Token: 0x04002273 RID: 8819
            pedIdleWaiting,
            // Token: 0x04002274 RID: 8820
            pedIdleChill,
            // Token: 0x04002275 RID: 8821
            pedIdleListen,
            // Token: 0x04002276 RID: 8822
            pedIdlePhone,
            // Token: 0x04002277 RID: 8823
            pedIdlePhone2,
            // Token: 0x04002278 RID: 8824
            pedIdleSquat,
            // Token: 0x04002279 RID: 8825
            pedIdleCheering,
            // Token: 0x0400227A RID: 8826
            pedIdleApplause,
            // Token: 0x0400227B RID: 8827
            pedIdleSitRelaxed,
            // Token: 0x0400227C RID: 8828
            pedIdleSitRelaxed2,
            // Token: 0x0400227D RID: 8829
            pedIdleSitRelaxed3,
            // Token: 0x0400227E RID: 8830
            pedIdleSitNewspaper,
            // Token: 0x0400227F RID: 8831
            pedIdleSitPhone2,
            // Token: 0x04002280 RID: 8832
            pedIdleSitPhone3,
            // Token: 0x04002281 RID: 8833
            pedIdleSitTense,
            // Token: 0x04002282 RID: 8834
            pedIdleTalk,
            // Token: 0x04002283 RID: 8835
            pedIdleTalk2,
            // Token: 0x04002284 RID: 8836
            pedIdleWaiting2,
            // Token: 0x04002285 RID: 8837
            pedIdlePockets,
            // Token: 0x04002286 RID: 8838
            birdIdle,
            // Token: 0x04002287 RID: 8839
            birdIdle2,
            // Token: 0x04002288 RID: 8840
            birdIdle3,
            // Token: 0x04002289 RID: 8841
            birdIdleSit,
            // Token: 0x0400228A RID: 8842
            birdIdleEat,
            // Token: 0x0400228B RID: 8843
            birdIdleGround,
            // Token: 0x0400228C RID: 8844
            dogIdle,
            // Token: 0x0400228D RID: 8845
            dogIdleSit,
            // Token: 0x0400228E RID: 8846
            dogIdleBark,
            // Token: 0x0400228F RID: 8847
            dogIdleLie,
            // Token: 0x04002290 RID: 8848
            dogIdleSleep,
            // Token: 0x04002291 RID: 8849
            pedIdleCheering2,
            // Token: 0x04002292 RID: 8850
            pedIdleLeaning,
            // Token: 0x04002293 RID: 8851
            pedIdleLeaning2,
            // Token: 0x04002294 RID: 8852
            pedIdleLeaning3,
            // Token: 0x04002295 RID: 8853
            pedIdleLeaning4,
            // Token: 0x04002296 RID: 8854
            pedIdleDrunkLeaning,
            // Token: 0x04002297 RID: 8855
            pedIdleDrunk,
            // Token: 0x04002298 RID: 8856
            pedSitDrunk,
            // Token: 0x04002299 RID: 8857
            pedSitGround,
            // Token: 0x0400229A RID: 8858
            pedSitGround2,
            // Token: 0x0400229B RID: 8859
            pedSitGround3,
            // Token: 0x0400229C RID: 8860
            catIdle,
            // Token: 0x0400229D RID: 8861
            catSit,
            // Token: 0x0400229E RID: 8862
            catLay,
            // Token: 0x0400229F RID: 8863
            catSleep
        }

        // Token: 0x02000468 RID: 1128
        public enum MoveAwayAnimation {
            // Token: 0x040022A1 RID: 8865
            pedMoveAway,
            // Token: 0x040022A2 RID: 8866
            pedMoveAwayFall,
            // Token: 0x040022A3 RID: 8867
            pedMoveAwayScared,
            // Token: 0x040022A4 RID: 8868
            birdMoveAway,
            // Token: 0x040022A5 RID: 8869
            dogMoveAway,
            // Token: 0x040022A6 RID: 8870
            pedMoveAwaySpin,
            // Token: 0x040022A7 RID: 8871
            catMoveAway
        }

        // Token: 0x02000469 RID: 1129
        public enum WalkAnimation {
            // Token: 0x040022A9 RID: 8873
            pedWalk,
            // Token: 0x040022AA RID: 8874
            pedWalkSassy,
            // Token: 0x040022AB RID: 8875
            pedWalkPhone,
            // Token: 0x040022AC RID: 8876
            pedWalkStrut,
            // Token: 0x040022AD RID: 8877
            pedWalkPockets,
            // Token: 0x040022AE RID: 8878
            pedWalkPockets2,
            // Token: 0x040022AF RID: 8879
            pedWalkPhoneSassy
        }
    }
}
