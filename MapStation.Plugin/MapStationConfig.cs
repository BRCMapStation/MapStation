using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Configuration;

namespace MapStation.Plugin {
    public class MapStationConfig {
        public static MapStationConfig Instance { get; private set; }

        // doing conditional compilation here cause we're probs gonna have settings to test growth and progress stuff and we don't want to let people cheat so easily lol
#if MAPSTATION_DEBUG
#if UNITY_EDITOR
        public bool QuickLaunchValue => false;
        public bool DisableKBMInputValue => false;
        public bool DebugUIValue => false;
        public bool ShowRedDebugShapesValue => false;
#else
        public ConfigEntry<bool> QuickLaunch;
        public ConfigEntry<bool> DisableKBMInput;
        public ConfigEntry<bool> DebugUI;
        public ConfigEntry<bool> ShowRedDebugShapes;

        public bool QuickLaunchValue => QuickLaunch.Value;
        public bool DisableKBMInputValue => DisableKBMInput.Value;
        public bool DebugUIValue => DebugUI.Value;
        public bool ShowRedDebugShapesValue => ShowRedDebugShapes.Value;
#endif
#endif

        public MapStationConfig() {
            Instance = this;
        }
        public MapStationConfig(ConfigFile file) : this() {
#if MAPSTATION_DEBUG && !UNITY_EDITOR
            QuickLaunch = file.Bind(
                "Development",
                "QuickLaunch",
                false,
                "Skip game intros and menu and launch directly into a map."
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
            ShowRedDebugShapes = file.Bind(
                "Development",
                "ShowRedDebugShapes",
                true,
                "Show the semi-transparent red meshes from Unity Editor along grind lines, etc."
            );
#endif
        }
    }
}