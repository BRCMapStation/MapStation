using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(CameraZone))]
[CanEditMultipleObjects]
public class CameraZoneEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Camera-Zone");
        DrawDefaultInspector();
    }
}
