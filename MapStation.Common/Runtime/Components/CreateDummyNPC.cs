using Reptile;
using UnityEngine;

namespace MapStation.Common.Components {
    /// <summary>
    /// Cyphers *must* have a child NPC component,
    /// even though it doesn't need to be enabled and doesn't necessary do anything.
    /// 
    /// This component will create a dummy NPC component on startup, to keep vanilla BRC happy.
    /// </summary>
    public class CreateDummyNPC : MonoBehaviour {
#if BEPINEX
        void Awake() {
            gameObject.SetActive(false);
            gameObject.AddComponent<NPC>();
        }
#endif
    }
}
