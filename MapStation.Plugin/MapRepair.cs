using Reptile;
using UnityEngine;

namespace MapStation.Plugin {
    /// <summary>
    /// After map loads, will automatically add missing gameobjects or components to avoid crashes.
    /// Where possible, fixing issues this way is easier than requiring mappers to use the doctor and fix manually.
    /// </summary>
    public class MapRepair {
        public static void OnStagePreInitialization(Stage newStage, Stage previousStage) {
            if (StageEnum.IsKnownMapId(newStage)) {
                Repair();
            }
        }
        
        public static void Repair() {
            // Create a default spawner if the map has no spawners
            if (GameObject.FindObjectOfType<PlayerSpawner>() == null) {
                var go = new GameObject();
                var spawner = go.AddComponent<PlayerSpawner>();
                spawner.isDefaultSpawnPoint = true;
            }
            
            // Create disabled, dummy NPC in each Cypher missing one
            foreach(var css in GameObject.FindObjectsOfType<CharacterSelectSpot>()) {
                var npc = css.GetComponentInChildren<NPC>(includeInactive:true);
                if (npc == null) {
                    var dummyNpcGo = new GameObject("DummyNPC");
                    dummyNpcGo.transform.parent = css.transform;
                    dummyNpcGo.SetActive(false);
                    dummyNpcGo.AddComponent<NPC>();
                }
            }
        }
    }
}
