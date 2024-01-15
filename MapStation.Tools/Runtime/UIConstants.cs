public static class UIConstants {
    public const string hotkeyGroupName = "BRC MapStation";
    public const string menuLabel = "BRC MapStation";
    public const string experimentsSubmenuLabel = "Experiments";
    public const string GizmoIconBaseDir = "Packages/com.brcmapstation.tools/Assets/Icons";

    // Unity rule: add 11 or more to get a separator
    // Unity quirk: Submenus get "stuck" at a certain priority until you restart Unity Editor.
    public enum MenuOrder {
        BUILD_ASSETS = 0,
        BUILD_ASSETS_AND_RUN_ON_STEAM,
        BUILD_AND_PACKAGE_FOR_THUNDERSTORE,
        //---
        UPDATE_PLUGIN = 20,
        //---
        MAP_PROPERTIES = 40,
        DOCTOR,
        GRIND_INSPECTOR,
        PREFERENCES,
        //---
        EXPORT_TO_OBJ = 60,
        MAKE_STAGE_PROXY_MESH,
        //---
        EXPERIMENTS = 80
    }
}
