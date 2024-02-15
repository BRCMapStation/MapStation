using System;
using System.ComponentModel;
using MapStation.Common.Doctor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.EditorGUILayout;
using static UnityEngine.GUILayout;
using static UnityEngine.GUI;
using static cspotcode.UnityGUI.GUIUtil;
using cspotcode.UnityGUI;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class DoctorWindow : EditorWindow {
    const string windowLabel = "Doctor";

    [MenuItem(UIConstants.menuLabel + "/" + windowLabel, priority = (int)UIConstants.MenuOrder.DOCTOR)]
    public static DoctorWindow ShowWindow() {
        var wnd = GetWindow<DoctorWindow>();
        wnd.titleContent = new GUIContent(windowLabel);
        return wnd;
    }

    private Analysis analysis;
    private bool[] showDiag;
    private bool[] showGo;
    private Vector2 scrollPosition = Vector2.zero;
    private Texture2D drPoloTexture;
    private GUIContent warningIcon;
    private GUIContent errorIcon;

    void OnEnable() {
        drPoloTexture = (Texture2D)AssetDatabase.LoadAssetAtPath(UIConstants.DrPoloIcon, typeof(Texture2D));
        // List of all Unity icons: https://github.com/halak/unity-editor-icons
        warningIcon = EditorGUIUtility.IconContent("Warning");
        errorIcon = EditorGUIUtility.IconContent("Error");
    }

    public void Analyze() {
        Analysis = Doctor.Analyze();
    }
    
    public Analysis Analysis {
        get => analysis;
        set {
            analysis = value;
            showDiag = new bool[analysis.diagnostics.Count];
            showGo = new bool[analysis.gameObjects.Count + 1];
        }
    }

    private void OnGUI() {
        GUIStyle wrappedLabel = new GUIStyle(skin.label) {
            wordWrap = true
        };
        GUIStyle boldLabel = new GUIStyle(skin.label) {
            fontStyle = FontStyle.Bold
        };
        HelpBox(new GUIContent() {
            image = drPoloTexture,
            text = Doctor.AboutMe,
        });
        if (Button("Analyze")) {
            Analyze();
        }
        if (analysis != null) {
            Label($"Analysis found {analysis.diagnostics.Count} problems.", boldLabel);
            Label(new GUIContent() {
                image = getIconForSeverity(Severity.Error).image,
                text = $"{analysis.countBySeverity[Severity.Error]} errors"
            });
            Label(new GUIContent() {
                image = getIconForSeverity(Severity.Warning).image,
                text = $"{analysis.countBySeverity[Severity.Warning]} warnings"
            });
            Space();
            using (ScrollView(ref scrollPosition)) {
                var di = -1;
                var goi = 0;
                if (analysis.diagnosticsWithoutTarget.Count > 0) {
                    Label("Misc");
                    using (Indent(apply:true)) {
                        foreach (var diagnostic in analysis.diagnosticsWithoutTarget) {
                            DrawDiagnostic(diagnostic);
                        }
                    }
                }
                foreach(var pair in analysis.gameObjects) {
                    goi++;
                    var gameObject = pair.Key;
                    var diagnostics = pair.Value;
                    showGo[goi] = BeginFoldoutHeaderGroup(showGo[goi], $"{diagnostics[0].TargetPath}");
                    EndFoldoutHeaderGroup(); // Unity doesn't let us nest them, so we close it immediately
                    if(showGo[goi]) {
                        EditorGUILayout.ObjectField("Game Object", gameObject, typeof(GameObject), allowSceneObjects: true);
                        using (Indent(apply:true)) {
                            foreach (var diagnostic in diagnostics) {
                                DrawDiagnostic(diagnostic);
                            }
                        }
                    }
                }
                
                void DrawDiagnostic(Diagnostic diagnostic) {
                    di++;
                    showDiag[di] = BeginFoldoutHeaderGroup(showDiag[di], new GUIContent() {
                        image = getIconForSeverity(diagnostic.Severity).image,
                        text = diagnostic.Message
                    });
                    EndFoldoutHeaderGroup();
                    if (showDiag[di]) {
                        using (Indent(apply:true)) {
                            Label(diagnostic.Details == null ? "<no details>" : diagnostic.Details, wrappedLabel);
                        }
                    }
                }
            }
        }
    }

    private GUIContent getIconForSeverity(Severity Severity) {
        switch (Severity) {
            case Severity.Error:
                return errorIcon;
            case Severity.Warning:
                return warningIcon;
            default:
                return null;
        }
    }
}
