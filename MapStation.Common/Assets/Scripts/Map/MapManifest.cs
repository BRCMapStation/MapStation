using System;

/// <summary>
/// Do not change field names of this class; because it will break the JSON structure.
/// </summary>
[Serializable]
class MapManifestJson {
    /// <summary>
    /// Mapping file format used by this map.
    /// If future versions of MapStation make major changes to the structure of a map, then newer maps will use a
    /// higher version number.
    /// </summary>
    public int version = 1;

    /// <summary>
    /// Globally unique internal id for this map.
    /// We strongly recommend prefixing with your username to avoid accidental conflicts with other map authors.
    /// For example, if cspotcode creates a vacation island map, "internalName" can be "cspotcode.island"
    /// </summary>
    public string internalName;

    /// <summary>
    /// Name of the map, appears in-game in menus and UI.
    /// </summary>
    public string displayName;

    /// <summary>
    /// Map author's name, appears in-game in menus and UI.
    /// </summary>
    string authorName;

    /// <summary>
    /// If true, this map is shown in fast-travel menus, such as the subway car, phone app, or taxi menu.
    /// Set to false if this map should only be accessible through portals from other maps, story quests, etc.
    /// </summary>
    bool showInFastTravelMenus;
}
