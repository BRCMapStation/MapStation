using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(WallrunLine))]
public class WallrunLineEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Wallrun");
        DrawDefaultInspector();
    }
}
