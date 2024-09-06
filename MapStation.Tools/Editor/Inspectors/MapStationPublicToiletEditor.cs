using MapStation.Common.Runtime.Gameplay;
using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(MapStationPublicToilet))]
[CanEditMultipleObjects]
public class MapStationPublicToiletEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Public-Toilet");
        DrawDefaultInspector();
    }
}
