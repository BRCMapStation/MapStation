using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    [ExecuteAlways]
    public class DialogueBranch : MonoBehaviour {
        public Sequence Sequence;
        public WinterObjective RequiredObjective;
        public Condition Condition;

        private void Awake() {
            if (!Application.isEditor)
                return;
            hideFlags = HideFlags.HideInInspector;
        }

        public bool Test() {
            switch (Condition) {
                case Condition.None:
                    break;
                case Condition.HasWantedLevel:
                    if (!WantedManager.instance.Wanted)
                        return false;
                    break;
            }
            if (RequiredObjective != null) {
                if (WinterProgress.Instance.LocalProgress.Objective != RequiredObjective)
                    return false;
            }
            return true;
        }
    }
}
