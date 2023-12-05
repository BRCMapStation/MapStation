using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    public class StageObject : MonoBehaviour {
        public void PutInChunk() {
            var chunks = FindObjectsOfType<StageChunk>(true);
            foreach(var chunk in chunks) {
                if (chunk == null)
                    continue;
                var colliders = chunk.GetComponentsInChildren<BoxCollider>(true);
                foreach(var chunkCollider in colliders) {
                    if (chunkCollider == null)
                        continue;
                    if (StageChunk.PointInOBB(transform.position, chunkCollider)) {
                        transform.SetParent(chunk.transform);
                        return;
                    }
                }
            }
        }
    }
}
