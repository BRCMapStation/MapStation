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
#else
        public ConfigEntry<bool> QuickLaunch;
        public ConfigEntry<bool> DisableKBMInput;

        public bool QuickLaunchValue => QuickLaunch.Value;
        public bool DisableKBMInputValue => DisableKBMInput.Value;
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
#endif
        }
    }
}
