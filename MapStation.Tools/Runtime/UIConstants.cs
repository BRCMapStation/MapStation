public static class UIConstants {
    public const string hotkeyGroupName = "BRC MapStation";
    public const string menuLabel = "BRC MapStation";
    public const string mapactionsSubmenuLabel = "Map Actions";
    public const string experimentsSubmenuLabel = "Experiments";
    public const string GizmoIconBaseDir = "Packages/com.brcmapstation.tools/Assets/Icons";
    public const string DrPoloIcon = GizmoIconBaseDir + "/drpoloIcon.png";

    // Unity rule: add 11 or more to get a separator
    // Unity quirk: Submenus get "stuck" at a certain priority until you restart Unity Editor.
    public enum MenuOrder {
        BUILD_CURRENT_ASSETS = 0,
        BUILD_CURRENT_ASSETS_AND_RUN_ON_STEAM,
        BUILD_CURRENT_ASSETS_AND_PACKAGE_FOR_THUNDERSTORE,
        //---
        BUILD_ASSETS = 20,
        BUILD_ASSETS_AND_RUN_ON_STEAM,
        BUILD_AND_PACKAGE_FOR_THUNDERSTORE,
        //---
        CREATE_MAP = 40,
        UPDATE_PLUGIN,
        EXPORT_TUTORIAL_MAP,
        //---
        MAP_PROPERTIES = 80,
        MAP_ACTIONS,
        DOCTOR,
        GRIND_INSPECTOR,
        EDIT_MINIMAP,
        PREFERENCES,
        //---
        DOCUMENTATION = 100,
        //---
        EXPORT_TO_OBJ = 120,
        MAKE_STAGE_PROXY_MESH,
        //---
        EXPERIMENTS = 140
    }
}
