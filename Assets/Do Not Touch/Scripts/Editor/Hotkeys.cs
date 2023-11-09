using UnityEditor.ShortcutManagement;
using UnityEngine;

/// <summary>
/// global hotkey bindings, logic should live elsewhere
/// </summary>
static class Hotkeys {
    [Shortcut(Constants.hotkeyGroupName + "/Rebuild Map", KeyCode.F5)]
    static void RebuildAll() {
        new BRCMapBuilder(Object.FindFirstObjectByType<BRCMap>()).RebuildAll();
    }
}