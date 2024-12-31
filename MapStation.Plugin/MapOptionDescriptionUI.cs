using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace MapStation.Plugin {
    public class MapOptionDescriptionUI : MonoBehaviour {
        public static MapOptionDescriptionUI Instance { get; private set; }
        private TextMeshProUGUI _descLabel;

        private void Awake() {
            Instance = this;
            Init();
        }

        static TextMeshProUGUI MakeLabel(TextMeshProUGUI reference, string name) {
            var font = reference.font;
            var fontSize = reference.fontSize;
            var fontMaterial = reference.fontMaterial;

            var labelObj = new GameObject(name);
            var newLabel = labelObj.AddComponent<TextMeshProUGUI>();
            newLabel.font = font;
            newLabel.fontSize = fontSize;
            newLabel.fontMaterial = fontMaterial;
            newLabel.alignment = TextAlignmentOptions.MidlineLeft;
            newLabel.fontStyle = FontStyles.Bold;
            newLabel.outlineWidth = 0.2f;

            return newLabel;
        }

        void Init() {
            var rectParent = Instance.gameObject.AddComponent<RectTransform>();
            rectParent.anchorMin = Vector2.zero;
            rectParent.anchorMax = Vector2.one;

            var uiManager = Core.Instance.UIManager;
            var labels = uiManager.danceAbilityUI.GetComponentsInChildren<TextMeshProUGUI>(true);
            TextMeshProUGUI referenceText = null;
            foreach (var label in labels) {
                if (label.transform.gameObject.name == "DanceSelectConfirmText")
                    referenceText = label;
            }

            if (referenceText == null)
                return;

            _descLabel = MakeLabel(referenceText, "DescLabel");
            _descLabel.text = "";
            _descLabel.rectTransform.anchorMin = new Vector2(0.5f, 1.0f);
            _descLabel.rectTransform.anchorMax = new Vector2(0.5f, 1.0f);
            _descLabel.rectTransform.pivot = new Vector2(0f, 1f);
            _descLabel.rectTransform.anchoredPosition = new Vector2(-100f, -100f);
            _descLabel.rectTransform.sizeDelta = new Vector2(500f, 500f);
            _descLabel.horizontalAlignment = HorizontalAlignmentOptions.Center;
            _descLabel.verticalAlignment = VerticalAlignmentOptions.Top;
            _descLabel.rectTransform.SetParent(rectParent, false);
        }

        public void SetText(string text) {
            _descLabel.text = text;
        }

        public static void Create() {
            if (Instance != null)
                Destroy(Instance.gameObject);
            var go = new GameObject("Map Option Description");
            go.transform.SetParent(Core.Instance.UIManager.gameplay.transform.parent.GetComponent<RectTransform>(), false);
            go.AddComponent<MapOptionDescriptionUI>();
            go.SetActive(false);
        }
    }
}
