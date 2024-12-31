using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapStation.Common.Runtime {
    public class LoadedMapOptions {
        public Dictionary<string, string> Options;
        public static Func<LoadedMapOptions> GetCurrentMapOptions;

        public void MakeDefault() {
            Options.Clear();
            var mapOptions = MapOptions.Instance;
            if (mapOptions == null) return;
            foreach(var option in mapOptions.Options) {
                Options[option.Name] = option.DefaultValue;
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
