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
        public void SetOnScrewpole(SkateboardScrewPole screwPole) {
            PlantedOn = screwPole.point.transform;
        }
        
        // 
        public void UpdateAbility() {
        }

        public void FixedUpdateAbility() {
        }

        /// <summary>
        /// Called by Player.LateUpdateAnimation
        /// *if* player is active, *whether or not* player is handplanting.
        /// </summary>
        public void LateUpdateAnimation() {
            var p = ReptileHandplantAbility.p;
            // If currently handplanting
            if(p.ability == this.ReptileHandplantAbility && this.PlantedOn != null) {
                var position = PlantedOn.position + Vector3.up * 0.03f;
                // Directly manipulate position to avoid rigidbody delaying update till next FixedUpdate
                // I hope this doesn't cause issues
                p.transform.position = position;
            }
        }
    }
}
