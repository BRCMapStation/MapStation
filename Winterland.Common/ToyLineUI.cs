using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

namespace Winterland.Common {
    public class ToyLineUI : MonoBehaviour {
        public bool Visible {
            get {
                return gameObject.activeSelf;
            }

            set {
                if (value && !gameObject.activeSelf)
                    gameObject.SetActive(true);
                else if (!value && gameObject.activeSelf)
                    gameObject.SetActive(false);
            }
        }

        public void SetCounter(int currentAmount, int maxAmount) {
            toyLineLabel.text = $"{currentAmount}/{maxAmount}";
        }

        private void Awake() {
            Visible = false;
        }

        [SerializeField]
        private TextMeshProUGUI toyLineLabel = null;
    }
}
