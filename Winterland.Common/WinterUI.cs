using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public class WinterUI : MonoBehaviour {
        public static WinterUI Instance { get; private set; }
        public ToyLineUI ToyLineUI = null;
        public FauxJuggleUI FauxJuggleUI = null;

        private void Awake() {
            Instance = this;
        }
    }
}
