
using UnityEditor;
using UnityEngine;

public class PreferencesWindow : EditorWindow {
    const string windowLabel = "Preferences";

    [MenuItem(Constants.menuLabel + "/" + windowLabel)]
    private static void ShowMyEditor() {
        EditorWindow wnd = GetWindow<PreferencesWindow>();
        wnd.titleContent = new GUIContent(windowLabel);
    }

    private void OnInspectorUpdate() {
        Repaint();
    }

    private Vector2 scrollbarPosition = Vector2.zero;

    private void OnGUI() {

        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 2;
        // EditorGUIUtility.fieldWidth = EditorGUIUtility.currentViewWidth / 3;

        scrollbarPosition = EditorGUILayout.BeginScrollView(scrollbarPosition, false, false);

        Preferences.instance.GetInstanceID();
        var e = Editor.CreateEditor(Preferences.instance);
        e.OnInspectorGUI();

        EditorGUILayout.EndScrollView();
    }
}