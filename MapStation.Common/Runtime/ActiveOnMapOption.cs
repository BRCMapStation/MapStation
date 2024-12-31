using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MapStation.Common.Runtime {
    public class ActiveOnMapOption : MonoBehaviour {
        public enum FilterModes {
            OptionMatches,
            OptionDoesntMatch
        }
        public FilterModes FilterMode = FilterModes.OptionMatches;
        public string OptionName;
        public string[] OptionValues;
    }
}
