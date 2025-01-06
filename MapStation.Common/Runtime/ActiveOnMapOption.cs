using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MapStation.Common.Runtime {
    public class ActiveOnMapOption : MonoBehaviour {
        public enum Errors {
            InvalidOptionName,
            InvalidOptionValue,
            NoOptionValues,
            NoMapOptions,
            NoError
        }
        public enum FilterModes {
            OptionMatchesAnyValue,
            OptionDoesntMatchAnyValue
        }
        public FilterModes FilterMode = FilterModes.OptionMatchesAnyValue;
        public string OptionName;
        public string[] OptionValues;

        public static string GetErrorString(Errors error) {
            switch (error) {
                case Errors.InvalidOptionName:
                    return "Invalid Option.";

                case Errors.InvalidOptionValue:
                    return "One or more option values are invalid.";

                case Errors.NoMapOptions:
                    return "There are no Map Options present in the map.";

                case Errors.NoOptionValues:
                    return "No option values were defined.";

                default:
                    return error.ToString();
            }
        }

        public Errors GetError() {
            var mapOptions = MapOptions.Instance;
            if (mapOptions == null) {
                return Errors.NoMapOptions;
            }
            if (mapOptions.Options == null) {
                return Errors.InvalidOptionName;
            }
            MapOptions.MapOption foundOption = null;
            foreach(var option in mapOptions.Options) {
                if (option.Name == OptionName) {
                    foundOption = option;
                    break;
                }
            }
            if (foundOption == null) {
                return Errors.InvalidOptionName;
            }
            if (OptionValues == null || OptionValues.Length <= 0)
                return Errors.NoOptionValues;
            foreach(var val in OptionValues) {
                if (!foundOption.PossibleValues.Contains(val))
                    return Errors.InvalidOptionValue;
            }
            return Errors.NoError;
        }

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
