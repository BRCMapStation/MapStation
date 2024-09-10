using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(Pickup))]
[CanEditMultipleObjects]
public class PickupEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Pickup");
        DrawDefaultInspector();
    }
}
