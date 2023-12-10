using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Configuration;

namespace Winterland.Common {
    public class WinterConfig {
        public static WinterConfig Instance { get; private set; }

        // doing conditional compilation here cause we're probs gonna have settings to test growth and progress stuff and we don't want to let people cheat so easily lol
#if WINTER_DEBUG
#if UNITY_EDITOR
        public bool QuickLaunchValue => false;
        public bool DisableKBMInputValue => false;
        public bool DebugUIValue => false;
        public bool ResetLocalSaveValue => false;
        public bool MockGlobalProgressLocallyValue => false;
        public float MockGlobalProgressStartTreeAtValue => 0;
#else
        public ConfigEntry<bool> QuickLaunch;
        public ConfigEntry<bool> DisableKBMInput;
        public ConfigEntry<bool> DebugUI;
        public ConfigEntry<bool> ResetLocalSave;
        public ConfigEntry<bool> MockGlobalProgressLocally;
        public ConfigEntry<float> MockGlobalProgressStartTreeAt;

        public bool QuickLaunchValue => QuickLaunch.Value;
        public bool DisableKBMInputValue => DisableKBMInput.Value;
        public bool DebugUIValue => DebugUI.Value;
        public bool ResetLocalSaveValue => ResetLocalSave.Value;
        public bool MockGlobalProgressLocallyValue => MockGlobalProgressLocally.Value;
        public float MockGlobalProgressStartTreeAtValue => MockGlobalProgressStartTreeAt.Value;
#endif
#endif

        public WinterConfig() {
            Instance = this;
        }
        public WinterConfig(ConfigFile file) : this() {
#if WINTER_DEBUG && !UNITY_EDITOR
            QuickLaunch = file.Bind(
                "Development",
                "QuickLaunch",
                false,
                "Skip game intros and menu and launch directly into Millenium Square."
            );
            DisableKBMInput = file.Bind(
                "Development",
                "DisableKBMInput",
                false,
                "Disable keyboard and mouse inputs, making it easier to use Unity Explorer or tab between windows. If this is enabled, you MUST use a game controller."
            );
            DebugUI = file.Bind(
                "Development",
                "DebugUI",
                true,
                "Show IMGui debug UI"
            );
            ResetLocalSave = file.Bind(
                "Development",
                "ResetLocalSave",
                false,
                "Reset our local Winterland progress."
            );
            MockGlobalProgressLocally = file.Bind(
                "Development",
                "MockGlobalProgressLocally",
                false,
                "Do *not* sync global event progress to SlopCrew, for local testing."
            );
            MockGlobalProgressStartTreeAt = file.Bind(
                "Development",
                "MockGlobalProgressStartTreeAt",
                0f,
                "When mocking global progress locally, start the tree at this percentage construction. (decimal number, a float)"
            );
#endif
        }
    }
}
