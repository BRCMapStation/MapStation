using Reptile;
using UnityEngine;

namespace MapStation.Plugin.Gameplay {
    public class MapStationHandplantAbility {
        public HandplantAbility ReptileHandplantAbility { get; private set; } = null;

        public Transform PlantedOn { get; private set; }

        public MapStationHandplantAbility(HandplantAbility reptileHandplantAbility) {
            ReptileHandplantAbility = reptileHandplantAbility;
        }

        public void SetToPole(Transform plantOn) {
            PlantedOn = plantOn;
            ReptileHandplantAbility.SetToPole(plantOn.position);
        }
        
        // 
        public void UpdateAbility() {
            var p = ReptileHandplantAbility.p;
            // If currently handplanting
            if(p.ability == this.ReptileHandplantAbility) {
                p.motor.SetPositionTeleport(PlantedOn.position + Vector3.up * 0.03f);
            }
        }

        public void FixedUpdateAbility() {
            
        }
    }
}
