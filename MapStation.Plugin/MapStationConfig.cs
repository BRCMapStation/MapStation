using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Configuration;

namespace MapStation.Plugin {
    public class MapStationConfig {
        public static MapStationConfig Instance { get; private set; }

#if BEPINEX
        public ConfigEntry<string> QuickLaunch;
        public string QuickLaunchValue => QuickLaunch.Value;

        public ConfigEntry<bool> QuickReload;
        public bool QuickReloadValue => QuickReload.Value;

        public ConfigEntry<bool> DisableKBMInput;
        public bool DisableKBMInputValue => DisableKBMInput.Value;

        public ConfigEntry<bool> DebugUI;
        public bool DebugUIValue => DebugUI.Value;

        public ConfigEntry<bool> ShowDebugShapes;
        public bool ShowDebugShapesValue => ShowDebugShapes.Value;

#if MAPSTATION_DEBUG
        // Options only for developers, not for mappers
        // <none>
#endif

#else
        // Stub everything as disabled, so we don't need as much conditional
        // compilation elsewhere.

        public bool QuickLaunchValue => "";
        public bool QuickReloadValue => true;
        public bool DisableKBMInputValue => false;
        public bool DebugUIValue => false;
        public bool ShowDebugShapesValue => false;

        // Options only for developers, not for mappers
        // <none>
#endif

        public MapStationConfig() {
            Instance = this;
        }
        public MapStationConfig(ConfigFile file) : this() {
#if BEPINEX
            var mappingSection = "Mapping";
            QuickLaunch = file.Bind(
                mappingSection,
                "QuickLaunch",
                "",
                "Skip game intros and menu and launch directly into this map."
            );
            QuickReload = file.Bind(
                mappingSection,
                "QuickReload",
                true,
                "If enabled, F5 will instantly reload the current map. Useful for rapidly testing map changes."
            );
            DisableKBMInput = file.Bind(
                mappingSection,
                "DisableKBMInput",
                false,
                "Disable keyboard and mouse inputs, making it easier to use Unity Explorer or tab between windows. If this is enabled, you MUST use a game controller."
            );
            DebugUI = file.Bind(
                mappingSection,
                "DebugUI",
                false,
                "Show Debug UI in the corner."
            );
            ShowDebugShapes = file.Bind(
                mappingSection,
                "ShowDebugShapes",
                false,
                "Show debug meshes along grind lines, spawners, teleporters, etc."
            );
#if MAPSTATION_DEBUG
            // Options only for developers, not for mappers
#endif
#endif
        }
    }
}
