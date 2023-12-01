using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Configuration;

namespace Winterland.Common {
    public class WinterConfig {
        public static WinterConfig Instance { get; private set; }

#if WINTER_DEBUG
        public ConfigEntry<bool> QuickLaunch;
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
#endif
        }
    }
}
