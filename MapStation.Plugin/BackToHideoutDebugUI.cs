using Reptile;
using UnityEngine;
using MapStation.Common;

namespace MapStation.Plugin;

class BackToHideoutDebugUI {
    const string Name = "Hideout Escape";

    public void Register(DebugUI ui) {
        ui.RegisterMenu(Name, OnGUI);
    }

    private void OnGUI() {
        if(Plugin.CanSwitchStagesWithoutCrashing()) {
            GUILayout.Label("This button takes you back to the hideout, if you get trapped in a custom map.");
            if(GUILayout.Button("Go to Hideout")) {
                Core.Instance.BaseModule.SwitchStage(Stage.hideout);
            }
        }
    }
}