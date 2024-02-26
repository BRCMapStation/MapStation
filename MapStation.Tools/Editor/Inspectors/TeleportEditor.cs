using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(Teleport))]
public class TeleportEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Teleporter");
        DrawDefaultInspector();
    }
}
