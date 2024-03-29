using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(SkateboardScrewPole))]
[CanEditMultipleObjects]
public class SkateboardScrewPoleEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Skateboard-Screw-Pole");
        DrawDefaultInspector();
    }
}
