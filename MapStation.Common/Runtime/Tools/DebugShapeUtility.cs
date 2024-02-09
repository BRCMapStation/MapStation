using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MapStation.Common {
    
    /// <summary>
    /// DEPRECATED TODO remove
    /// </summary>
    public class DebugShapeUtility {
        public static IEnumerable<Renderer> FindDebugShapes(GameObject root) {
            var renderers = root.GetComponentsInChildren<Renderer>(includeInactive: true);
            return renderers.Where(r => r.sharedMaterial?.name == "Debug");
        }

        public static void SetDebugShapesVisibility(GameObject root, bool visible) {
            foreach(var renderer in FindDebugShapes(root)) {
                renderer.enabled = visible;
            }
        }
    }
}
