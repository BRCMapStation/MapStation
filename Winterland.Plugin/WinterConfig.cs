using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Configuration;

namespace Winterland.Plugin {
    public class WinterConfig {
        public ConfigEntry<bool> QuickLaunch;
        public WinterConfig(ConfigFile file) {
            QuickLaunch = file.Bind(
                "Development",
                "QuickLaunch",
                false,
                "Skip game intros and menu and launch directly into Millenium Square."
            );
        }
    }
}
