using System.Collections.Generic;
using System.Linq;
using Reptile;
using UnityEditor;
using UnityEngine;
using MapStation.Components;
using System;

public class GrindWindow : EditorWindow
{
    const string WindowLabel = "Grind Inspector";

    [MenuItem(UIConstants.menuLabel + "/" + WindowLabel, priority = (int)UIConstants.MenuOrder.GRIND_INSPECTOR)]
    private static void ShowMyEditor() {
        // This method is called when the user selects the menu item in the Editor
        EditorWindow wnd = GetWindow<GrindWindow>();
        wnd.titleContent = new GUIContent(WindowLabel);

        // Limit size of the window
        wnd.minSize = new Vector2(450, 200);
        wnd.maxSize = new Vector2(1920, 720);
    }

    Grind[] grinds = {};
    GrindPath[] grindPaths = {};
    GrindNode[] grindNodes = {};
    GrindLine[] grindLines = {};
    SplineBasedGrindLineGenerator[] splineBasedGrindLineGenerators = {};
    BezierSpline[] bezierSplines = {};

    private Editor grindEditor = null;
    private Editor grindPathEditor = null;
    private Editor grindNodeEditor = null;
    private Editor grindNodeTransformsEditor = null;
    private Editor grindLineEditor = null;
    private Editor splineBasedGrindLineGeneratorEditor = null;
    private Editor bezierSplineEditor = null;

    private Vector2 inspectorScroll = Vector2.zero;

    private void OnEnable() {
        // delegate registration must be on OnEnable.
        // OnEnable is called after domain reload; Awake is not
        Selection.selectionChanged -= onSelectionChanged;
        Selection.selectionChanged += onSelectionChanged; 
        refreshSelectedComponentsAndEditors();
    }

    private void OnDestroy() {
        Selection.selectionChanged -= onSelectionChanged;
        DestroyImmediate(grindEditor);
        DestroyImmediate(grindPathEditor);
        DestroyImmediate(grindNodeEditor);
        DestroyImmediate(grindNodeTransformsEditor);
        DestroyImmediate(grindLineEditor);
        DestroyImmediate(splineBasedGrindLineGeneratorEditor);
        DestroyImmediate(bezierSplineEditor);
    }

    private void onSelectionChanged() {
        refreshSelectedComponentsAndEditors();
    }

    private void refreshSelectedComponentsAndEditors() {
        var aos = Selection.gameObjects;
        grinds = GetComponentInParentOfEach<Grind>(aos);
        Editor.CreateCachedEditor(grinds, null, ref grindEditor);

        if(grinds.Length == 0) {
            grindPaths = new GrindPath[] {};
            grindNodes = new GrindNode[] {};
            grindLines = new GrindLine[] {};
            splineBasedGrindLineGenerators = new SplineBasedGrindLineGenerator[] {};
            bezierSplines = new BezierSpline[] {};
        } else {
            grindPaths = GetComponentInParentOfEach<GrindPath>(aos);
            grindNodes = GetComponentInParentOfEach<GrindNode>(aos);
            grindLines = GetComponentInParentOfEach<GrindLine>(aos);
            splineBasedGrindLineGenerators = GetComponentInParentOfEach<SplineBasedGrindLineGenerator>(aos);
            bezierSplines = GetComponentInParentOfEach<BezierSpline>(aos);

            Editor.CreateCachedEditor(grindPaths, null, ref grindPathEditor);
            Editor.CreateCachedEditor(grindNodes, null, ref grindNodeEditor);
            var transforms = grindNodes.Select(x => x.transform).ToArray();
            Editor.CreateCachedEditor(transforms, null, ref grindNodeTransformsEditor);
            Editor.CreateCachedEditor(grindLines, null, ref grindLineEditor);
            Editor.CreateCachedEditor(splineBasedGrindLineGenerators, null, ref splineBasedGrindLineGeneratorEditor);
            Editor.CreateCachedEditor(bezierSplines, null, ref bezierSplineEditor);
        }
    }

    /// <summary>
    /// Prevents console errors when trying render inspector for destroyed object
    /// </summary>
    private bool anyInspectedComponentIsDestroyed() {
        bool FindDestroyed(Component[] components) => Array.Exists(components, c => c == null);
        return
            FindDestroyed(grindPaths)
            || FindDestroyed(grindNodes)
            || FindDestroyed(grindLines)
            || FindDestroyed(splineBasedGrindLineGenerators)
            || FindDestroyed(bezierSplines);
    }

    static T[] GetComponentInParentOfEach<T>(GameObject[] objects) {
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


    private void OnInspectorUpdate() {
        Repaint();
    }

    private void OnGUI()
    {
        if(anyInspectedComponentIsDestroyed()) {
            refreshSelectedComponentsAndEditors();
        }

        // Restore defaults; we modify these elsewhere
        EditorGUIUtility.labelWidth = 0;
        EditorGUIUtility.fieldWidth = 0;

        EditorGUI.BeginChangeCheck();

        void Header(string label, int height = 1) {
            GUILayout.Space(10);
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            Rect rect = EditorGUILayout.GetControlRect(false, height);
            rect = EditorGUI.IndentedRect(rect);
            EditorGUI.DrawRect(rect, new Color(0.4f, 0.4f, 0.4f));
            GUILayout.Space(2);
        }

        inspectorScroll = EditorGUILayout.BeginScrollView(inspectorScroll);

        if(grinds.Length == 0) {
            EditorGUILayout.HelpBox("Select any grind to see inspectors.  This panel will show only Grind, GrindPath, GrindNode, and GrindLine inspectors.", MessageType.Info);
        } else {
            Header("Grind");
            grindEditor.OnInspectorGUI();

            if(grindPaths.Length != 0) {
                Header("Grind Path");
                grindPathEditor.OnInspectorGUI();
            }
            if(grindNodes.Length != 0) {
                Header("Grind Node");
                if(Preferences.instance.grinds.showNodeTransformsInGrindInspector) {
                    grindNodeTransformsEditor.OnInspectorGUI();
                }
                grindNodeEditor.OnInspectorGUI();
            }
            if(splineBasedGrindLineGenerators.Length != 0) {
                Header("Spline Based Grind Line Generator");
                splineBasedGrindLineGeneratorEditor.OnInspectorGUI();
            }
            if(bezierSplines.Length != 0) {
                Header("Bezier Spline");
                bezierSplineEditor.OnInspectorGUI();
            }
            if(grindLines.Length != 0) {
                Header("Grind Line");
                grindLineEditor.OnInspectorGUI();
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
