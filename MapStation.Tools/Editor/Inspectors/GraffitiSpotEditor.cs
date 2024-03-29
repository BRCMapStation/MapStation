using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(GraffitiSpot))]
[CanEditMultipleObjects]
public class GraffitiSpotEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Graffiti-Spot");
        DrawDefaultInspector();
    }
}
