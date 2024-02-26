using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(GraffitiSpot))]
public class GraffitiSpotEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Graffiti-Spot");
        DrawDefaultInspector();
    }
}
