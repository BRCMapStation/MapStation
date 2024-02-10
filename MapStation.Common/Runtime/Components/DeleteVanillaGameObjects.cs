using System;
using System.Collections.Generic;
using UnityEngine;
using MapStation.Common.Serialization;
using Reptile;

namespace MapStation.Common.VanillaAssets {
    /// <summary>
    /// This component deletes GameObjects by path, allowing us to remove things from the base map.
    /// We can also replace things from the base map by deleting them and shipping a replacement in our prefab.
    /// </summary>
    public class DeleteVanillaGameObjectsV1 : MonoBehaviour {

        public static DeleteVanillaGameObjectsV1 Instance { get; private set; }
        // We can't just destroy some things because by the time we inject our prefab a bunch of other things depend on them, such as streetlife.
        // To work around this we just disable them, and make a patch so that they don't get re-activated if they're on this hashset.
        private HashSet<GameObject> disabledGameObjects = new HashSet<GameObject>();
        // BepInEx serialization workaround
        private List<Deletion> Deletions => deletions.items;
        [SerializeReference] private SList_Deletion deletions = new ();
        public class SList_Deletion : SList<Deletion> {}

        private void Awake() {
            if(!Application.isEditor) {
#if BEPINEX
                Instance = this;
                DeleteGameObjects();
#endif
            }
        }

#if BEPINEX
        public void DeleteGameObjects() {
            foreach(var d in Deletions) {
                var go = GameObject.Find(d.Path);
                if(go == null) {
                    Log.Info($"{nameof(DeleteVanillaGameObjectsV1)} could not find GameObject to delete at path: {d.Path}");
                } else {
                    HandleDeleteGameObject(go);
                }
            }
        }

        // Putting special cases here for things that might crash or break the game in some way if deleted.
        private void HandleDeleteGameObject(GameObject gameObject) {
            var streetLifeCluster = gameObject.GetComponent<StreetLifeCluster>();
            if (streetLifeCluster != null) {
                DisableGameObject(gameObject);
                return;
            }
            Destroy(gameObject);
        }

        private void DisableGameObject(GameObject gameObject) {
            gameObject.SetActive(false);
            disabledGameObjects.Add(gameObject);
        }

        public bool IsDisabled(GameObject gameObject) {
            if (disabledGameObjects.Contains(gameObject))
                return true;
            return false;
        }
#endif

        [Serializable]
        public class Deletion {
            public string Path;
        }
    }
}
