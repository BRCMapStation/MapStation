using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(AmbientManager))]
public class AmbientManagerEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Ambient-Manager");
        DrawDefaultInspector();
    }
}
