using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MapStation.Common.Runtime {
    public class MapOptions : MonoBehaviour {
        public MapOption[] Options;

        [Serializable]
        public class MapOption {
            public string Name;
            [TextArea(5,5)]
            public string Description;
            public string DefaultValue;
            public string[] PossibleValues;
            public Camera PreviewCamera;
        }
    }
}
