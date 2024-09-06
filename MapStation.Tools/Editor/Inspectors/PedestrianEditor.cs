using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(Pedestrian))]
[CanEditMultipleObjects]
public class PedestrianEditor : Editor {
    private static string[] PedestrianNames = {
        "Business Man"
    };
    private static int[] PedestrianIds = { 
        12
    };
    private static string[] PedestrianMeshes = {
        "meshBusiness2"
    };
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Pedestrian");
        DrawDefaultInspector();
    }
}
