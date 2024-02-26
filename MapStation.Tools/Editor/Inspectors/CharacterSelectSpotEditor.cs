using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(CharacterSelectSpot))]
public class CharacterSelectSpotEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Cypher");
        DrawDefaultInspector();
    }
}
