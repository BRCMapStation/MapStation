using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    /// <summary>
    /// This object will be automatically put into a StageChunk. Useful for components that depend on chunks, like Junk.
    /// </summary>
    public class StageObject : MonoBehaviour {
        [Header("Name of the chunk GameObject. Leave this empty to automatically find a chunk.")]
        [SerializeField]
        private string chunkName = "";
        public void PutInChunk() {
            var myChunk = GetComponentInParent<StageChunk>();
            if (myChunk != null)
                return;
            var parentStageObject = GetComponentInParent<StageObject>();
            if (parentStageObject != this) {
                parentStageObject.PutInChunk();
                return;
            }
            var chunks = FindObjectsOfType<StageChunk>(true);
            foreach(var chunk in chunks) {
                if (chunk == null)
                    continue;
                if (!string.IsNullOrEmpty(chunkName)) {
                    if (chunk.gameObject.name == chunkName)
                        transform.SetParent(chunk.transform);
                    else
                        continue;
                }
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
