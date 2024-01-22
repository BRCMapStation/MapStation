using System;
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
    private static void ShowMyEditor() {
        EditorWindow wnd = GetWindow<DoctorWindow>();
        wnd.titleContent = new GUIContent(windowLabel);
    }
    
    private Analysis analysis;
    private bool[] showDiag;
    private bool[] showGo;
    private Vector2 scrollPosition = Vector2.zero;
    private ObjectWrapper wrapper;

    void OnEnable() {
        if(wrapper != null) DestroyImmediate(wrapper);
        wrapper = ScriptableObject.CreateInstance<ObjectWrapper>();
    }

    private void OnDestroy() {
        if(wrapper != null) DestroyImmediate(wrapper);
    }

    private void OnGUI() {
        GUIStyle wrappedLabel = new GUIStyle(skin.label) {
            wordWrap = true
        };
        GUIStyle boldLabel = new GUIStyle(skin.label) {
            fontStyle = FontStyle.Bold
        };
        EditorGUILayout.HelpBox(Doctor.AboutMe, MessageType.Info);
        if (Button("Analyze")) {
            analysis = Doctor.Analyze();
            showDiag = new bool[analysis.diagnostics.Count];
            showGo = new bool[analysis.gameObjects.Count + 1];
        }
        if (analysis != null) {
            Label($"Analysis found {analysis.diagnostics.Count} problems.", boldLabel);
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
                var serializedObject = new SerializedObject(wrapper);
                var prop = serializedObject.Prop(nameof(wrapper.gameObject));
                foreach(var pair in analysis.gameObjects) {
                    goi++;
                    var gameObject = pair.Key;
                    var diagnostics = pair.Value;
                    showGo[goi] = BeginFoldoutHeaderGroup(showGo[goi], $"{diagnostics[0].TargetPath}");
                    EndFoldoutHeaderGroup(); // Unity doesn't let us nest them, so we close it immediately
                    if(showGo[goi]) {
                        wrapper.gameObject = gameObject;
                        serializedObject.Update();
                        EditorGUILayout.ObjectField(prop);
                        using (Indent(apply:true)) {
                            foreach (var diagnostic in diagnostics) {
                                DrawDiagnostic(diagnostic);
                            }
                        }
                    }
                }
                
                void DrawDiagnostic(Diagnostic diagnostic) {
                    di++;
                    showDiag[di] = BeginFoldoutHeaderGroup(showDiag[di], diagnostic.Message);
                    EndFoldoutHeaderGroup();
                    if (showDiag[di]) {
                        using (Indent(apply:true)) {
                            Label(diagnostic.Details == null ? "<no details>" : diagnostic.Details);
                        }
                    }
                }
            }
        }
    }
    
    [Serializable]
    class ObjectWrapper : ScriptableObject {
        [FormerlySerializedAs("GameObject")]
        [SerializeReference]
        public GameObject gameObject;
    }
}
