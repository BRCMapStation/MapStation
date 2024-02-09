using BepInEx.Configuration;
using UnityEngine;

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
        public ConfigEntry<KeyCode> QuickReloadKey;
        public KeyCode QuickReloadKeyValue => QuickReloadKey.Value;
        public ConfigEntry<KeyCode> DisableKBMInputKey;
        public KeyCode DisableKBMInputKeyValue => DisableKBMInputKey.Value;

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
        public bool QuickReloadKeyValue => KeyCode.F5;
        public bool DisableKBMInputKeyValue => KeyCode.F6;

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
            // Keeping descriptions adjacent so I remember to update them at same time

            var quickReloadDescription = "Enable to reload *any* map with a hotkey (F5 by default), including vanilla and downloaded maps. By default, only locally-built maps can be reloaded.";
            var quickReloadKeyDescription = "Reloads the map.";
            QuickReload = file.Bind(
                mappingSection,
                "QuickReload",
                true,
                quickReloadDescription
            );
            // Keeping descriptions adjacent so I remember to update them at same time
            var disableKBMInputDescription = "Disable keyboard and mouse input, making it easier to use MapStation's DebugUI, Unity Explorer, or tab between windows. When this is enabled, you MUST use a game controller. Note: there is also a hotkey for this.";
            var disableKBMInputKeyDescription = "Enables/disables keyboard and mouse input.";
            DisableKBMInput = file.Bind(
                mappingSection,
                "DisableKBMInput",
                false,
                disableKBMInputDescription
            );
            DebugUI = file.Bind(
                mappingSection,
                "DebugUI",
                false,
                "Show Debug UI in the corner for *all* maps. By default this is enabled only for locally-built maps."
            );
            ShowDebugShapes = file.Bind(
                mappingSection,
                "ShowDebugShapes",
                false,
                "Show debug meshes along grind lines, spawners, teleporters, etc."
            );

            var inputSection = "Input";
            QuickReloadKey = file.Bind(
                inputSection,
                "QuickReloadKey",
                KeyCode.F5,
                quickReloadKeyDescription
            );
            DisableKBMInputKey = file.Bind(
                inputSection,
                "DisableKBMInputKey",
                KeyCode.F6,
                disableKBMInputKeyDescription
            );
#if MAPSTATION_DEBUG
            // Options only for developers, not for mappers
#endif
#endif
        }
    }
}
