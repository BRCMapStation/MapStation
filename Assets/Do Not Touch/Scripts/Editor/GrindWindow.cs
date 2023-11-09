using System.Collections.Generic;
using System.Linq;
using Reptile;
using UnityEditor;
using UnityEngine;

public class GrindWindow : EditorWindow
{
    const string windowLabel = "Grind Inspector";

    [MenuItem(Constants.menuLabel + "/" + windowLabel)]
    private static void ShowMyEditor() {
        // This method is called when the user selects the menu item in the Editor
        EditorWindow wnd = GetWindow<GrindWindow>();
        wnd.titleContent = new GUIContent(windowLabel);

        // Limit size of the window
        wnd.minSize = new Vector2(450, 200);
        wnd.maxSize = new Vector2(1920, 720);
    }

    private void OnInspectorUpdate() {
        Repaint();
    }

    private Vector2 inspectorScroll = Vector2.zero;

    private void OnGUI()
    {
        // Restore defaults; we modify these elsewhere
        EditorGUIUtility.labelWidth = 0;
        EditorGUIUtility.fieldWidth = 0;

        EditorGUI.BeginChangeCheck();

        var aos = Selection.gameObjects;

        T[] GetComponentInParentOfEach<T>(GameObject[] objects) {
            List<T> results = new List<T>();
            foreach(var o in objects) {
                try {
                    var c = o.GetComponentInParent<T>(includeInactive: true);
                    if(c != null && !results.Contains(c)) results.Add(c);
                } catch(MissingReferenceException) {
                    // Swallow these, because sometimes inspectors delete objects
                }
            }
            return results.ToArray();
        }

        void Header(string label, int height = 1) {
            GUILayout.Space(10);
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            Rect rect = EditorGUILayout.GetControlRect(false, height);
            rect = EditorGUI.IndentedRect(rect);
            EditorGUI.DrawRect(rect, new Color(0.4f, 0.4f, 0.4f));
            GUILayout.Space(2);
        }

        void ShowEditor(Object[] objects) {
            var e = Editor.CreateEditor(objects);
            e.OnInspectorGUI();
            // DestroyImmediate(e);
        }

        inspectorScroll = EditorGUILayout.BeginScrollView(inspectorScroll);

        var grinds = GetComponentInParentOfEach<Grind>(aos);
        if(grinds.Count() == 0) {
            EditorGUILayout.HelpBox("Select any grind to see inspectors.  This panel will show only Grind, GrindPath, GrindNode, and GrindLine inspectors.", MessageType.Info);
        } else {
            Header("Grind");
            ShowEditor(grinds);

            var grindPaths = GetComponentInParentOfEach<GrindPath>(aos);
            if(grindPaths.Count() != 0) {
                Header("Grind Path");
                ShowEditor(grindPaths);
            }
            var grindNodes = GetComponentInParentOfEach<GrindNode>(aos);
            if(grindNodes.Count() != 0) {
                Header("Grind Node");
                if(Preferences.instance.grinds.showNodeTransformsInGrindInspector) {
                    var transforms = grindNodes.Select(x => x.transform).ToArray();
                    ShowEditor(transforms);
                }
                ShowEditor(grindNodes);
            }
            var splineBasedGrindLineGenerators = GetComponentInParentOfEach<SplineBasedGrindLineGenerator>(aos);
            if(splineBasedGrindLineGenerators.Count() != 0) {
                Header("Spline Based Grind Line Generator");
                ShowEditor(splineBasedGrindLineGenerators);
            }
            var bezierSplines = GetComponentInParentOfEach<BezierSpline>(aos);
            if(bezierSplines.Count() != 0) {
                Header("Bezier Spline");
                ShowEditor(bezierSplines);
            }
            var grindLines = GetComponentInParentOfEach<GrindLine>(aos);
            if(grindLines.Count() != 0) {
                Header("Grind Line");
                ShowEditor(grindLines);
            }
        }

        EditorGUILayout.EndScrollView();

        EditorGUI.EndChangeCheck();
    }

    void OnSceneGUI() {
        // Delegate to inspectors' OnSceneGUI for the selection, not for parent gameobjects(?)
        // The inspector does this, we must mimic the inspector.
        // For example, splines rely on OnSceneGUI for their editing.
    }
}
