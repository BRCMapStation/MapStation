using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

namespace Winterland.Common {
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ToyLineLabel : MonoBehaviour {
        public Color32 CompletedLineFontColor;
        private TextMeshProUGUI text = null;
        private Color32 defaultFontColor;

        private void Awake() {
            text = GetComponent<TextMeshProUGUI>();
            defaultFontColor = text.faceColor;
        }

        public void SetCounter(int amount, int maximum) {
            text.text = $"{amount}/{maximum}";
            if (amount == maximum)
                text.faceColor = CompletedLineFontColor;
            else
                text.faceColor = defaultFontColor;
        }
    }
}
