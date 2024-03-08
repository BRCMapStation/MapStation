using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(AmbientTrigger))]
[CanEditMultipleObjects]
public class AmbientTriggerEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Ambient-Trigger");
        DrawDefaultInspector();
    }
}
