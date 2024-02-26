using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(PlayerSpawner))]
public class PlayerSpawnerEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Spawners");
        DrawDefaultInspector();
    }
}
