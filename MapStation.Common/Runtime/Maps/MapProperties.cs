using System;
using System.Collections.Generic;

namespace MapStation.Common {
    /// <summary>
    /// Saved as properties.json in .brcmap
    /// We can read this metadata *without* loading map asset bundles!
    /// 
    /// **Do not change field names of this class!**
    /// It will break the JSON structure.
    /// Instead, use [SerializeField] w/ a public property getter/setter to use a different name in C# than JSON.
    /// </summary>
    [Serializable]
    public class MapProperties {
    
        // Alternative names considered:
        // manifest, configuration, metadata, properties
        //
        // "Manifest" already refers to Thunderstore zip manifest, might cause confusion
        // "Configuration" already refers to BepInEx plugin configs, might cause confusion

        /// <summary>
        /// Mapping file format used by this map.
        /// If future versions of MapStation make major changes to the structure of a map, then newer maps will use a
        /// higher version number.
        /// </summary>
        public int format = 1;

        /// <summary>
        /// Globally unique internal id for this map.
        /// We strongly recommend prefixing with your username to avoid accidental conflicts with other map authors.
        /// For example, if cspotcode creates a vacation island map, "internalName" can be "cspotcode.island"
        /// </summary>
        public string internalName;

        /// <summary>
        /// Version of this map, required for publishing to Thunderstore.
        /// </summary>
        public string version = "0.0.0";

        /// <summary>
        /// Name of the map, appears in-game in menus and UI.
        /// </summary>
        public string displayName;

        /// <summary>
        /// Map author's name, appears in-game in menus and UI.
        /// </summary>
        public string authorName;

        /// <summary>
        /// If true, this map is shown in fast-travel menus, such as the subway car, phone app, or taxi menu.
        /// Set to false if this map should only be accessible through portals from other maps, story quests, etc.
        /// </summary>
        public bool showInFastTravelMenus;
    }
}