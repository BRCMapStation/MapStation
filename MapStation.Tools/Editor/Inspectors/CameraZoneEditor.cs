using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(CameraZone))]
public class CameraZoneEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Camera-Zone");
        DrawDefaultInspector();
    }
}
