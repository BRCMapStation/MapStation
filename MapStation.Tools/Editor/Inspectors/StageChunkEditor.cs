using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(StageChunk))]
[CanEditMultipleObjects]
public class StageChunkEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Stage-Chunk");
        DrawDefaultInspector();
    }
}
