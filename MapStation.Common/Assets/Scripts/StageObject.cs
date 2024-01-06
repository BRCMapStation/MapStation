using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace MapStation.Common {
    /// <summary>
    /// This object will be automatically put into a StageChunk. Useful for components that depend on chunks, like Junk.
    /// </summary>
    public class StageObject : MonoBehaviour {
        [Header("Name of the chunk GameObject. Leave this empty to automatically find a chunk.")]
        [SerializeField]
        private string chunkName = "";

#if BEPINEX
        public void PutInChunk() {
            var myChunk = GetComponentInParent<StageChunk>(true);
            if (myChunk != null)
                return;
            var parentStageObject = GetComponentInParent<StageObject>(true);
            if (parentStageObject != this && parentStageObject != null) {
                parentStageObject.PutInChunk();
                return;
            }
            var chunks = FindObjectsOfType<StageChunk>(true);
            StageChunk closestChunk = null;
            float closestChunkDistance = 0f;
            foreach(var chunk in chunks) {
                if (chunk == null)
                    continue;
                // We might have some custom chunks for specific cases, and don't want shit automatically being put into them.
                if (chunk.gameObject.name.StartsWith("!"))
                    continue;
                if (!string.IsNullOrEmpty(chunkName)) {
                    if (chunk.gameObject.name == chunkName) {
                        transform.SetParent(chunk.transform);
                        return;
                    } else
                        continue;
                }
                var colliders = chunk.GetComponentsInChildren<BoxCollider>(true);
                var inoob = false;
                foreach(var chunkCollider in colliders) {
                    if (chunkCollider == null)
                        continue;
                    if (StageChunk.PointInOBB(transform.position, chunkCollider)) {
                        inoob = true;
                        break;
                    }
                }
                if (inoob) {
                    var dist = Vector3.Distance(transform.position, chunk.transform.position);
                    if (closestChunk == null) {
                        closestChunk = chunk;
                        closestChunkDistance = dist;
                    }
                    else if (dist < closestChunkDistance) {
                        closestChunk = chunk;
                        closestChunkDistance = dist;
                    }
                }
            }
            if (closestChunk != null) {
                transform.SetParent(closestChunk.transform);
            }
        }
#endif
    }
}