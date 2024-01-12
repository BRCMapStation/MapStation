
using MapStation.Common;
using UnityEditor;
using UnityEngine;
using cspotcode.UnityGUI;
using static UnityEditor.EditorGUILayout;
using static UnityEditor.EditorGUI;
using static UnityEngine.GUILayout;
using static UnityEngine.GUI;
using static cspotcode.UnityGUI.GUIUtil;
using System.Runtime.InteropServices;

public class PreferencesWindow : EditorWindow {
    const string WindowLabel = "Preferences";

    [MenuItem(UIConstants.menuLabel + "/" + WindowLabel, priority = UIConstants.defaultMenuPriority)]
    private static void ShowMyEditor() {
        EditorWindow wnd = GetWindow<PreferencesWindow>();
        wnd.titleContent = new GUIContent(WindowLabel);
    }

    private void OnInspectorUpdate() {
        Repaint();
    }

    private Vector2 scrollbarPosition = Vector2.zero;

    private Editor preferencesEditor = null;

    private void OnGUI() {

        EditorGUIUtility.labelWidth = 300;
        // EditorGUIUtility.fieldWidth = EditorGUIUtility.currentViewWidth / 2;

        using(ScrollView(ref scrollbarPosition)) {
            Editor.CreateCachedEditor(Preferences.instance, null, ref preferencesEditor);
            preferencesEditor.OnInspectorGUI();
        }
    }
}

[CustomEditor(typeof(Preferences))]
public class PreferencesEditor : Editor {
    private Preferences preferences => target as Preferences;

    Vector2 scrollPosition;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        foreach(var prefGroup in serializedObject.IterChildren()) {
            switch(prefGroup.name) {

                // General
                case nameof(preferences.general):
                    DrawGeneralPreferences(prefGroup);
                    break;

                // Everything else
                default:
                    prefGroup.Draw();
                    break;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawGeneralPreferences(SerializedProperty genPrefs) {
        foreach(var genPref in genPrefs.DrawSelfIterChildren()) {
            switch(genPref.name) {

                // Map directory
                case nameof(GeneralPreferences.testMapDirectory):
                    genPref.Draw();
                    using(ApplyIndent()) if(Button("Auto-detect")) {
                        // Remove user input focus so that assigned value appears immediately
                        FocusControl(null);
                        genPref.stringValue = PathConstants.AbsoluteTestMapsDirectoryFromBepInExProfile(PathDetection.GetBepInExProfileInRegistry());
                    }
                    break;

                // Everything else
                default:
                    genPref.Draw();
                    break;
            }
        }
    }
}
