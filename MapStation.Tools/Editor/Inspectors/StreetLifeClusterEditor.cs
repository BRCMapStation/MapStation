using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(StreetLifeCluster))]
[CanEditMultipleObjects]
public class StreetLifeClusterEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Street-Life-Cluster");
        DrawDefaultInspector();
    }
}
