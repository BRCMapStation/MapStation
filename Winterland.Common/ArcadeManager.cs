using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public class ArcadeManager : MonoBehaviour {
        public static ArcadeManager Instance { get; private set; }
        public List<Arcade> Arcades = [];

        private void Awake() {
            Instance = this;
        }

        public void RegisterArcade(Arcade arcade) {
            Arcades.Add(arcade);
        }

        public void UpdateArcades() {
            foreach (var arcade in Arcades)
                arcade.UpdateAvailability();
        }
    }
}
