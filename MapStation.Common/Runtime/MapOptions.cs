using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MapStation.Common.Runtime {
    public class MapOptions : MonoBehaviour {
        public static MapOptions Instance {
            get {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<MapOptions>();
                return _instance;
            }
        }
        private static MapOptions _instance;
        public static Action OnMapOptionsChanged;
        public MapOption[] Options;
        private static ActiveOnMapOption[] ActiveOnMapOptions = [];

        [Serializable]
        public class MapOption {
            public string Name;
            [TextArea(5,5)]
            public string Description;
            public string DefaultValue;
            public string[] PossibleValues;
            [Tooltip("Camera shown when previewing the map option in-game.")]
            public Camera PreviewCamera;
        }

        private void Awake() {
            _instance = this;
        }

        public string GetDefaultOption(string optionName) {
            foreach(var option in Options) {
                if (option.Name == optionName)
                    return option.DefaultValue;
            }
            return string.Empty;
        }

        public static void UpdateActiveOnMapOptions() {
            foreach(var mapOption in ActiveOnMapOptions) {
                if (mapOption == null) continue;
                mapOption.UpdateActivation();
            }
        }

        public static void OnStageInitialized() {
            ActiveOnMapOptions = GameObject.FindObjectsOfType<ActiveOnMapOption>(true);
            UpdateActiveOnMapOptions();
        }
    }
}
