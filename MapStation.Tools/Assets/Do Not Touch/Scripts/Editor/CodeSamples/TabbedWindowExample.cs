using UnityEditor;
using UnityEngine;

/// <summary>
/// 
/// A dumping ground for code that makes a tabbed window UI.
/// 
/// Keeping for quick reference, we may need it.
/// 
/// </summary>
class TabbedWindowExample : EditorWindow {
    enum GrindWindowTab {
        Inspector = 0,
        Doctor = 1,
        Preferences = 2
    }

    private GrindWindowTab tab = GrindWindowTab.Inspector;

    private void OnGUI()
    {
        tab = (GrindWindowTab)GUILayout.Toolbar((int)tab, new string[] {
            GrindWindowTab.Inspector.ToString(),
            GrindWindowTab.Doctor.ToString(),
            GrindWindowTab.Preferences.ToString()
        });
        switch(tab) {
            case GrindWindowTab.Inspector:
                break;
            case GrindWindowTab.Doctor:
                break;
            case GrindWindowTab.Preferences:
                break;
        }
    }
}