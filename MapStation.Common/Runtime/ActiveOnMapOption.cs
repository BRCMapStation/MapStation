using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MapStation.Common.Runtime {
    public class ActiveOnMapOption : MonoBehaviour {
        public enum FilterModes {
            OptionMatchesAnyValue,
            OptionDoesntMatchAnyValue
        }
        public FilterModes FilterMode = FilterModes.OptionMatchesAnyValue;
        public string OptionName;
        public string[] OptionValues;

        public void UpdateActivation() {
            var mapOptions = LoadedMapOptions.GetCurrentMapOptions?.Invoke();
            if (mapOptions == null) return;
            if (ShouldActivate(mapOptions))
                gameObject.SetActive(true);
            else
                gameObject.SetActive(false);
        }

        private bool ShouldActivate(LoadedMapOptions mapOptions) {
            var matches = MatchesMapOptions(mapOptions);
            if (matches)
                return FilterMode == FilterModes.OptionMatchesAnyValue;
            else
                return FilterMode == FilterModes.OptionDoesntMatchAnyValue;
        }

        private bool MatchesMapOptions(LoadedMapOptions mapOptions) {
            if (mapOptions.OptionMatches(OptionName, OptionValues))
                return true;
            return false;
        }
    }
}
