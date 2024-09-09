using UnityEngine;

namespace Reptile
{
    public class CopterCopSpawner : BasicEnemySpawner, IInitializableSceneObject
    {
        // Token: 0x04000828 RID: 2088
        [SerializeField]
        private Transform startOffsetLocation;

        // Token: 0x04000829 RID: 2089
        [SerializeField]
        private LayerMask environmentBlockingLayers;

        // Token: 0x0400082A RID: 2090
        [SerializeField]
        private float maxVerticalRange = 30f;
    }
}
