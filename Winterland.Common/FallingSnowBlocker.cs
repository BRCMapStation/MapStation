using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    /// <summary>
    /// All colliders contained in this GameObject will block falling snow. Use in interiors, under roofing, etc. to prevent snow falling thru.
    /// </summary>
    public class FallingSnowBlocker : MonoBehaviour{
        private void Awake() {
            var blockers = GetComponentsInChildren<Collider>();
            var fallingSnowControllers = FindObjectsOfType<FallingSnowController>(true);
            foreach(var fallingSnowController in fallingSnowControllers) {
                foreach(var blocker in blockers) {
                    fallingSnowController.AddKillTrigger(blocker);
                }
            }
        }
    }
}
