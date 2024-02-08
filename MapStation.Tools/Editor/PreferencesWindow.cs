
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
using System;

public class PreferencesWindow : EditorWindow {
    const string WindowLabel = "Preferences";

    [MenuItem(UIConstants.menuLabel + "/" + WindowLabel, priority = (int)UIConstants.MenuOrder.PREFERENCES)]
    public static void ShowWindow() {
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
            Space();
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
                        var BepInExProfileDirectory = PathDetection.GetBepInExProfileInRegistry();
                        if(BepInExProfileDirectory == null || BepInExProfileDirectory == "") {
                            var title = "Cannot detect path to your BepInEx profile";
                            EditorUtility.DisplayDialog(
                                title,
                                "Unable to detect path to your BepInEx profile.\n" +
                                "\n" +
                                "Have you launched BRC with the MapStation plugin yet?\n" +
                                "Try installing the plugin and launching modded BRC first, then detect again.",
                                "Ok",
                                null);
                            throw new Exception(title);
                        }
                        genPref.stringValue = PathConstants.AbsoluteTestMapsDirectoryFromBepInExProfile(BepInExProfileDirectory);
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
