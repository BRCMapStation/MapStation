using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public class Arcade : MonoBehaviour {
        private void Awake() {
            ArcadeManager.Instance.RegisterArcade(this);
            UpdateAvailability();
        }
        public void UpdateAvailability() {
            gameObject.SetActive(WinterProgress.Instance.LocalProgress.ArcadeUnlocked);
        }
    }
}
