using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Reptile;
using UnityEngine;
using Winterland.Common;

namespace Winterland.Plugin.Patches {
    [HarmonyPatch(typeof(ComboMascot))]
    internal class ComboMascotPatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(ComboMascot.OnTriggerEnter))]
        private static bool OnTriggerEnter_Prefix(Collider other) {
            var winterPlayer = other.GetComponentInParent<WinterPlayer>();
            if (winterPlayer == null)
                return true;
            if (winterPlayer.CurrentToyLine != null)
                return false;
            return true;
        }
    }
}
