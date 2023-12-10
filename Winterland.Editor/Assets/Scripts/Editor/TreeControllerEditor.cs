using UnityEditor;
using UnityEngine;
using Winterland.Common;

[CustomEditor(typeof(TreeController))]
class TreeControllerEditor : Editor {
    private TreeController t => target as TreeController;

    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox(
            "In Unity Editor, you can press play to test tree construction animations.\n" +
            "When you press play, it will animate event progress from the start % to ending % you choose.",
            MessageType.None
        );
        EditorGUILayout.TextArea(
            "Pressing Play will:\n" +
            $"Start at {t.unityEditorPlayButtonStart * 100}% event progress, which is {t.unityEditorPlayButtonStart * TreeController.TimelineLength} seconds on the TreeConstruction timeline.\n" +
            $"Animate to {t.unityEditorPlayButtonEnd * 100}% event progress, which is {t.unityEditorPlayButtonEnd * TreeController.TimelineLength} seconds on the TreeConstruction timeline.",
            GUI.skin.label
        );
    }
}
