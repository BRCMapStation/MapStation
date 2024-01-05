using System;
using System.Collections.Generic;
using UnityEngine;
using MapStation.Common;

namespace MapStation.Tools {
    [CreateAssetMenu(fileName = "Properties", menuName = Constants.menuLabel + "/Map Properties", order = 1)]
    class MapPropertiesScriptableObject : ScriptableObject {
        [SerializeReference]
        MapProperties properties = new MapProperties();
    }
}
