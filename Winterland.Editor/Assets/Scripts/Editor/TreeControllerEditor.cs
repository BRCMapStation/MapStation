using UnityEditor;
using UnityEngine;
using Winterland.Common;

[CustomEditor(typeof(TreeController))]
class TreeControllerEditor : Editor {
    private TreeController t => target as TreeController;

    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        GUILayout.Label("Phases");
        foreach(var phase in t.treePhases) {
            GUILayout.Label(phase.gameObject.name);
            var newStartAt = EditorGUILayout.Slider(phase.StartAt, 0f, 1f);
            if(newStartAt != phase.StartAt) {
                Undo.RecordObject(phase, "Change TreePhase StartAt");
                phase.StartAt = newStartAt;
            }
        }
        // EditorGUILayout.HelpBox(
        //     "In Unity Editor, you can press play to test tree construction animations.\n" +
        //     "When you press play, it will animate event progress from the start % to ending % you choose.",
        //     MessageType.None
        // );
        // EditorGUILayout.TextArea(
        //     "Pressing Play will:\n" +
        //     $"Start at {t.unityEditorPlayButtonStart * 100}% event progress, which is {t.unityEditorPlayButtonStart * t.director.duration} seconds on the timeline.\n" +
        //     $"Animate to {t.unityEditorPlayButtonEnd * 100}% event progress, which is {t.unityEditorPlayButtonEnd * t.director.duration} seconds on the timeline.",
        //     GUI.skin.label
        // );
    }
}
