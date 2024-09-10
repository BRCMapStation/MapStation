using MapStation.Common.Runtime;
using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(MapStationCameraOverride))]
[CanEditMultipleObjects]
public class MapStationCameraOverrideEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Camera-Override");
        DrawDefaultInspector();
    }
}
