using System;
using System.Collections.Generic;
using UnityEngine;
using Winterland.MapStation.Common.Serialization;

namespace Winterland.MapStation.Common.VanillaAssets {
    /// <summary>
    /// This component deletes GameObjects by path, allowing us to remove things from the base map.
    /// We can also replace things from the base map by deleting them and shipping a replacement in our prefab.
    /// </summary>
    public class DeleteVanillaGameObjectsV1 : MonoBehaviour {

        // BepInEx serialization workaround
        private List<Deletion> Deletions => deletions.items;
        [SerializeReference] private SList_Deletion deletions = new ();
        public class SList_Deletion : SList<Deletion> {}

        private void Awake() {
            if(!Application.isEditor) {
                DeleteGameObjects();
            }
        }

        public void DeleteGameObjects() {
            foreach(var d in Deletions) {
                var go = GameObject.Find(d.Path);
                if(go == null) {
                    Debug.Log($"{nameof(DeleteVanillaGameObjectsV1)} could not find GameObject to delete at path: {d.Path}");
                } else {
                    Destroy(go);
                }
            }
        }
        [Serializable]
        public class Deletion {
            public string Path;
        }
    }
}
