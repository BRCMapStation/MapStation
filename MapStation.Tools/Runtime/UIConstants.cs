public static class UIConstants {
    public const string hotkeyGroupName = "BRC MapStation";
    public const string menuLabel = "BRC MapStation";
    public const string experimentsSubmenuLabel = "Experiments";
    public const string GizmoIconBaseDir = "Packages/com.brcmapstation.tools/Assets/Icons";
    public const string DrPoloIcon = GizmoIconBaseDir + "/drpoloIcon.png";

    // Unity rule: add 11 or more to get a separator
    // Unity quirk: Submenus get "stuck" at a certain priority until you restart Unity Editor.
    public enum MenuOrder {
        BUILD_ASSETS = 0,
        BUILD_ASSETS_AND_RUN_ON_STEAM,
        BUILD_AND_PACKAGE_FOR_THUNDERSTORE,
        //---
        CREATE_MAP = 20,
        UPDATE_PLUGIN,
        EXPORT_TUTORIAL_MAP,
        //---
        MAP_PROPERTIES = 40,
        DOCTOR,
        GRIND_INSPECTOR,
        PREFERENCES,
        //---
        DOCUMENTATION = 60,
        //---
        EXPORT_TO_OBJ = 80,
        MAKE_STAGE_PROXY_MESH,
        //---
        EXPERIMENTS = 100
    }
}
