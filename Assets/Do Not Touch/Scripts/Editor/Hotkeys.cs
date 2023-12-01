using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Reptile;

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

    [Shortcut(Constants.hotkeyGroupName + "/Add / Clone Grind Node(s)", typeof(SceneView), KeyCode.C)]
    static void AddGrindNodes() {
        // Implemented by GrindNodeEditor, so create one and delegate
        var aos = Selection.gameObjects;
        var grindNodes = aos.Select(go => go.GetComponent<GrindNode>()).Where(c => c != null).ToArray();
        if(grindNodes.Length > 0) {
            var e = Editor.CreateEditor(grindNodes) as GrindNodeEditor;
            if(e.CanDoAddNodeAction()) e.AddNodeAction();
            Object.DestroyImmediate(e);
        }
    }
}
