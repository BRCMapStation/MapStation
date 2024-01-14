using UnityEngine;

namespace MapStation.Common {
    /// <summary>
    /// Destroys self immediately, useful when Reptile code expects to spawn a "dynamic pickup"
    /// but we don't want that.
    /// </summary>
    public class NoOpDynamicPickup : MonoBehaviour {
        private void Awake() {
            Destroy(gameObject);
        }
    }
}