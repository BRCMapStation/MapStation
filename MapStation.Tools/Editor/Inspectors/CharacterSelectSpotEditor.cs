using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(CharacterSelectSpot))]
[CanEditMultipleObjects]
public class CharacterSelectSpotEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Cypher");
        DrawDefaultInspector();
    }
}
