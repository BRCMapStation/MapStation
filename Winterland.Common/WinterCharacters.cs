using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Bootstrap;
using CrewBoomAPI;
using Reptile;

namespace Winterland.Common {
    public static class WinterCharacters {
        public static bool CrewBoomInitialized {
            get {
                if (!IsCrewBoomInstalled)
                    return false;
                return CheckCrewBoomAPI();
                static bool CheckCrewBoomAPI() {
                    return CrewBoomAPIDatabase.IsInitialized;
                }
            }
        }
        public const string CrewBoomGUID = "CrewBoom";
        public static Guid SantaGUID = new("225a1be0-80eb-4c30-823a-c6c80afd1711");
        private static bool IsCrewBoomInstalled = false;

        public static void Initialize() {
            IsCrewBoomInstalled = Chainloader.PluginInfos.Keys.Contains(CrewBoomGUID);
        }

        public static bool IsSanta(Characters character) {
            if (!CrewBoomInitialized) return false;

            return CrewBoomStep();

            bool CrewBoomStep() {
                if (CrewBoomAPIDatabase.GetUserGuidForCharacter((int)character, out var guid)) {
                    if (guid == SantaGUID)
                        return true;
                }
                return false;
            }
        }
    }
}
