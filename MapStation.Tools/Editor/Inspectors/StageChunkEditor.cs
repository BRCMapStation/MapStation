using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(StageChunk))]
public class StageChunkEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Stage-Chunk");
        DrawDefaultInspector();
    }
}
