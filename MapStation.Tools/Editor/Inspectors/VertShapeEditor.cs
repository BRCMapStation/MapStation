using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(VertShape))]
public class VertShapeEditor : Editor {
    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("This is the vanilla BRC vert component, NOT the custom MapStation vert.", MessageType.Info);
        EditorHelper.MakeDocsButton("Vert-Ramp");
        DrawDefaultInspector();
    }
}
