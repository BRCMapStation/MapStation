using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using UnityEngine;

namespace Winterland.Common {
    /// <summary>
    /// Holds custom data in players.
    /// </summary>
    public class WinterPlayer : MonoBehaviour {
        public ToyLine CurrentToyLine = null;

        public static WinterPlayer Get(Player player) {
            return player.GetComponent<WinterPlayer>();
        }
    }
}
