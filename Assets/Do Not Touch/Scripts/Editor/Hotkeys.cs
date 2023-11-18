using UnityEditor.ShortcutManagement;
using UnityEngine;

/// <summary>
/// global hotkey bindings, logic should live elsewhere
/// </summary>
static class Hotkeys {
    #if !WINTERLAND
    // Disable binding for winterland project
    [Shortcut(Constants.hotkeyGroupName + "/Rebuild Map", KeyCode.F5)]
    #endif
    static void RebuildAll() {
        new BRCMapBuilder(Object.FindFirstObjectByType<BRCMap>()).RebuildAll();
    }
}