using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Reptile {
    public class BasicEnemySpawner : MonoBehaviour, IInitializableSceneObject {

        public GameObject[] spawnableEnemies;

        // Token: 0x040007F0 RID: 2032
        [SerializeField]
        protected PoolableInstantiator poolableInstantiator;

        // Token: 0x040007F1 RID: 2033
        [SerializeField]
        protected bool spawnOutOfSight;

        // Token: 0x040007F2 RID: 2034
        [SerializeField]
        protected float spawnTimeOutDuration;

        // Token: 0x040007F3 RID: 2035
        [SerializeField]
        protected float spawnRange;

        // Token: 0x040007F4 RID: 2036
        [SerializeField]
        protected float maxVerticalSpawnRange;
    }
}
