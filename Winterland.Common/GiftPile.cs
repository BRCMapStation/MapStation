using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public class GiftPile : MonoBehaviour {
        public int MinimumGiftAmount = 1;
        private void Awake() {
            GiftPileManager.Instance.RegisterPile(this);
        }

        private void Start() {
            UpdateAvailability();
        }

        public void UpdateAvailability() {
            gameObject.SetActive(WinterProgress.Instance.LocalProgress.Gifts >= MinimumGiftAmount);
        }
    }
}
