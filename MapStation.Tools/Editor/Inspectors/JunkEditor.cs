using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(Junk))]
[CanEditMultipleObjects]
public class JunkEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Junk");
        DrawDefaultInspector();
    }
}
