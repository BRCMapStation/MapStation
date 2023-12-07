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
        public ConfigEntry<bool> QuickLaunch;
        public ConfigEntry<bool> DisableKBMInput;
        public ConfigEntry<bool> DebugUI;
#endif
        public WinterConfig() {
            Instance = this;
        }
        public WinterConfig(ConfigFile file) : this() {
#if WINTER_DEBUG
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
#endif
        }
    }
}
