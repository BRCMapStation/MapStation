using UnityEditor;
using UnityEngine;

public class DoctorWindow : EditorWindow {
    const string windowLabel = "Doctor";

    [MenuItem(Constants.menuLabel + "/" + windowLabel)]
    private static void ShowMyEditor() {
        EditorWindow wnd = GetWindow<DoctorWindow>();
        wnd.titleContent = new GUIContent(windowLabel);
    }

    private void OnGUI() {
        EditorGUILayout.HelpBox("The Doctor will analyze your map and list any problems, offering suggestions to fix them.", MessageType.Info);
        GUILayout.Button("Analyze (not implemented yet)");
    }
}