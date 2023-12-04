using System;
using System.Collections.Generic;
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
        private static bool IsCrewBoomInstalled = false;
        private static Guid WinterBelGUID = new("4572e982-13ca-4d12-933e-ff0317444078");
        private static Guid WinterRiseGUID = new("bdb2f2cd-4ff2-483c-b43f-9f23bb287c68");
        private static Guid WinterShineGUID = new("ae9bdb5e-663c-43b2-9567-f0d73daa6e86");
        private static Dictionary<Guid, Characters> BaseCharacterByCustomCharacter = new();
        private static Dictionary<Characters, Guid> CustomCharacterByBaseCharacter = new();

        public static void Initialize() {
            IsCrewBoomInstalled = Chainloader.PluginInfos.Keys.Contains(CrewBoomGUID);
            LinkCharacter(WinterBelGUID, BrcCharacter.Bel);
            LinkCharacter(WinterRiseGUID, BrcCharacter.Rise);
            LinkCharacter(WinterShineGUID, BrcCharacter.Shine);
        }

        public static void LinkCharacter(Guid crewBoomCharacter, BrcCharacter baseCharacter) {
            BaseCharacterByCustomCharacter[crewBoomCharacter] = (Characters) baseCharacter;
            CustomCharacterByBaseCharacter[(Characters) baseCharacter] = crewBoomCharacter;
        }

        public static Guid GetCustomCharacter(Characters baseCharacter) {
            if (!CrewBoomInitialized)
                return Guid.Empty;
            return CrewBoomStep();

            Guid CrewBoomStep() {
                if (!CustomCharacterByBaseCharacter.TryGetValue(baseCharacter, out var result))
                    return Guid.Empty;
                return result;
            }
        }

        public static Characters GetBaseCharacter(Characters customCharacter) {
            if (!CrewBoomInitialized)
                return Characters.NONE;
            return CrewBoomStep();

            Characters CrewBoomStep() {
                if (!CrewBoomAPIDatabase.GetUserGuidForCharacter((int) customCharacter, out var guid))
                    return Characters.NONE;
                if (!BaseCharacterByCustomCharacter.TryGetValue(guid, out var result))
                    return Characters.NONE;
                return result;
            }
        }
    }
}
