using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(Pedestrian))]
[CanEditMultipleObjects]
public class PedestrianEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Pedestrian");
        DrawDefaultInspector();
    }
}
