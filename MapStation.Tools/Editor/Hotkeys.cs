using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Reptile;
using MapStation.Components;

/// <summary>
/// global hotkey bindings, logic should live elsewhere
/// </summary>
static class Hotkeys {
    [Shortcut(UIConstants.hotkeyGroupName + "/Add / Clone Grind Node(s)", typeof(SceneView), KeyCode.C)]
    static void AddGrindNodes() {
        var aos = Selection.gameObjects;

        // Are any nodes selected?
        var grindNodes = aos.Select(go => go.GetComponent<GrindNode>()).Where(c => c != null).ToArray();
        if(grindNodes.Length > 0) {
            // Implemented by GrindNodeEditor, so create one and delegate
            var e = Editor.CreateEditor(grindNodes) as GrindNodeEditor;
            if(e.CanDoAddNodeAction()) e.AddNodeAction();
            Object.DestroyImmediate(e);
            return;
        }

        // No nodes selected.  Are any `Grind` selected?

        // TODO this should technically check for grind nodes in parents, too.  In that situation,
        // the Grind's "Add Node" button will still appear in grind inspector, so you'd expect the hotkey to work, too.
        var grinds = aos.Select(go => go.GetComponentInParent<Grind>()).Where(c => c != null).ToHashSet();
        foreach(var grind in grinds) {
            grind.AddNode();
        }
    }
}
