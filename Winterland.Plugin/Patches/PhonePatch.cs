using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using HarmonyLib;
using Reptile.Phone;
using Winterland.Common;
using Winterland.Common.Challenge;

namespace Winterland.Plugin.Patches {
    [HarmonyPatch(typeof(Phone))]
    internal class PhonePatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(Phone.OpenCloseHandeling))]
        private static bool OpenCloseHandeling_Prefix(Phone __instance) {
            var state = __instance.state;
            if (state != Phone.PhoneState.ON && ChallengeLevel.CurrentChallengeLevel != null)
                return false;
            return true;
        }
    }
}
