using UnityEngine;

namespace Reptile {
    // Token: 0x0200019A RID: 410
    public class PoliceTubeSpawner : BasicEnemySpawner {
        // Token: 0x04000A64 RID: 2660
        [SerializeField]
        private Animator animator;

        // Token: 0x04000A65 RID: 2661
        [SerializeField]
        private Transform spawnTransform;

        // Token: 0x04000A66 RID: 2662
        [SerializeField]
        private Transform mainTube;

        // Token: 0x04000A67 RID: 2663
        [SerializeField]
        private Transform tubeTop;

        // Token: 0x04000A68 RID: 2664
        [SerializeField]
        private Transform posA;

        // Token: 0x04000A69 RID: 2665
        [SerializeField]
        private Transform posB;

        // Token: 0x04000A6A RID: 2666
        [SerializeField]
        private Transform posC;

        // Token: 0x04000A6B RID: 2667
        [SerializeField]
        private Transform posD;

        // Token: 0x04000A6C RID: 2668
        [SerializeField]
        private AudioSource audioSource;

        // Token: 0x02000429 RID: 1065
        public enum TubeState {
            // Token: 0x04002102 RID: 8450
            CONCEALED,
            // Token: 0x04002103 RID: 8451
            DOWN,
            // Token: 0x04002104 RID: 8452
            UP,
            // Token: 0x04002105 RID: 8453
            UPTORETURN
        }
    }
}
