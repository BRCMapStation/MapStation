using Reptile;
using UnityEngine;
using MapStation.Common;

namespace MapStation.Plugin.Tools;

class BackToHideoutDebugUI : DebugUI.DebugMenu {
    public override string Name => "Hideout Escape";

    public override void OnGUI() {
        if(Plugin.CanSwitchStagesWithoutCrashing()) {
            GUILayout.Label("This button takes you back to the hideout, if you get trapped in a custom map.");
            if(GUILayout.Button("Go to Hideout")) {
                Core.Instance.BaseModule.SwitchStage(Stage.hideout);
            }
        }
    }
}
