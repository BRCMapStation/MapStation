using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    [RequireComponent(typeof(GraffitiSpot))]
    public class REPLessGraffiti : MonoBehaviour {
        public static bool IsREPLess(GraffitiSpot graffSpot) {
            if (graffSpot.GetComponent<REPLessGraffiti>() != null)
                return true;
            return false;
        }
    }
}
