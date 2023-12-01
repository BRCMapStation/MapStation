using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public static class ObjectiveDatabase {
        public static WinterObjective StartingObjective = null;
        public static void Initialize(AssetBundle bundle) {
            var objectives = bundle.LoadAllAssets<WinterObjective>();
            foreach(var objective in objectives) {
                if (objective.StartingObjective) {
                    StartingObjective = objective;
                    break;
                }
            }
        }
    }
}
