using MapStation.Tools.Editor;
using Reptile;
using UnityEditor;

[CustomEditor(typeof(VendingMachine))]
[CanEditMultipleObjects]
public class VendingMachineEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Vending-Machine");
        DrawDefaultInspector();
    }
}
