using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public static class ObjectiveDatabase {
        public static WinterObjective StartingObjective = null;
        private static Dictionary<string, WinterObjective> ObjectiveByName = null;
        public static void Initialize(AssetBundle bundle) {
            ObjectiveByName = new();
            var objectives = bundle.LoadAllAssets<WinterObjective>();
            foreach(var objective in objectives) {
                ObjectiveByName[objective.name] = objective;
                if (objective.StartingObjective)
                    StartingObjective = objective;
            }
        }

        public static WinterObjective GetObjective(string objectiveName) {
            if (ObjectiveByName.TryGetValue(objectiveName, out var result))
                return result;
            return null;
        }
    }
}
