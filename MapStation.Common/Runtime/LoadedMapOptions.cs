using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapStation.Common.Runtime {
    public class LoadedMapOptions {
        public Dictionary<string, string> Options = new();
        public static Func<LoadedMapOptions> GetCurrentMapOptions;

        public void MakeDefault() {
            if (Options == null)
                Options = new();
            Options.Clear();
            var mapOptions = MapOptions.Instance;
            if (mapOptions == null) return;
            if (mapOptions.Options == null) return;
            foreach(var option in mapOptions.Options) {
                Options[option.Name] = option.DefaultValue;
            }
        }

        public void Sanitize() {
            var mapOptions = MapOptions.Instance;
            if (mapOptions == null) return;
            foreach (var loadedoption in Options) {
                foreach(var option in mapOptions.Options) {
                    if (option.Name == loadedoption.Key) {
                        if (!option.PossibleValues.Contains(loadedoption.Value))
                            Options[loadedoption.Key] = option.DefaultValue;
                    }
                }
            }
        }

        public string GetOption(string optionName) {
            if (Options.TryGetValue(optionName, out var result))
                return result;
            var mapOptions = MapOptions.Instance;
            if (mapOptions != null)
                return MapOptions.Instance.GetDefaultOption(optionName);
            return string.Empty;
        }

        public bool OptionMatches(string optionName, string[] optionValues) {
            return optionValues.Contains(GetOption(optionName));
        }
    }
}
