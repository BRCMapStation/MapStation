using MapStation.Tools.Editor;
using MapStation.Common.Runtime;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(GameAudio))]
[CanEditMultipleObjects]
public class GameAudioEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Game-Audio");
        DrawDefaultInspector();
    }
}
