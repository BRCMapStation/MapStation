using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(AmbientManager))]
[CanEditMultipleObjects]
public class AmbientManagerEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Ambient-Manager");
        DrawDefaultInspector();
    }
}
