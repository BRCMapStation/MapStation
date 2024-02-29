using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapStation.Tools
{
    public abstract class AMapStationPlugin
    {
        public virtual string[] GetDependencies() {
            return new string[0];
        }
    }
}
