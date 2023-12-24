using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public class GiftPileManager : MonoBehaviour {
        public static GiftPileManager Instance { get; private set; }
        public List<GiftPile> GiftPiles = [];

        private void Awake() {
            Instance = this;
        }

        public void RegisterPile(GiftPile giftPile) {
            GiftPiles.Add(giftPile);
        }

        public void UpdatePiles() {
            foreach (var giftPile in GiftPiles)
                giftPile.UpdateAvailability();
        }
    }
}
