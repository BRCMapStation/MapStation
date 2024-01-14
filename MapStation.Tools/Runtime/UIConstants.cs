public static class UIConstants {
    public const string hotkeyGroupName = "BRC MapStation";
    public const string menuLabel = "BRC MapStation";
    public const string experimentsSubmenuLabel = "Experiments";
    public const string GizmoIconBaseDir = "Packages/com.brcmapstation.tools/Assets/Icons";

    public enum MenuOrder {
        BUILD_ASSETS = 0,
        BUILD_ASSETS_AND_RUN_ON_STEAM,
        UPDATE_PLUGIN = 10,
        MAP_PROPERTIES = 20,
        DOCTOR,
        GRIND_INSPECTOR,
        PREFERENCES,
        EXPORT_TO_OBJ = 40,
        MAKE_STAGE_PROXY_MESH,
        EXPERIMENTS = 60
    }
}
