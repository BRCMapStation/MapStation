using Reptile;
using UnityEngine;
using MapStation.Common;

namespace MapStation.Plugin.Tools;

class HiddenShapesDebugUI : DebugUI.DebugMenu {
    public override string Name => "Show/Hide Hidden Shapes";

    public override void OnGUI() {
        GUILayout.Label("Toggle visibility of hidden shapes: spawn points, grind lines, etc.");
        if(GUILayout.Button(HiddenShapes.Visible ? "Hide" : "Show")) {
            HiddenShapes.Visible = !HiddenShapes.Visible;
        }
    }
}
