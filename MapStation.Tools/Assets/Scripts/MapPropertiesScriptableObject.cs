using System;
using System.Collections.Generic;
using UnityEngine;
using MapStation.Common;

namespace MapStation.Tools {
    [CreateAssetMenu(fileName = "Properties", menuName = UIConstants.menuLabel + "/Map Properties", order = 1)]
    public class MapPropertiesScriptableObject : ScriptableObject {
        public MapProperties properties = new MapProperties();
    }
}
