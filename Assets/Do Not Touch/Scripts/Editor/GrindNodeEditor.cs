using System.Linq;
using Reptile;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GrindNode))]
[CanEditMultipleObjects]
class GrindNodeEditor : Editor
{
    private GrindNode[] grindNodes;
    private void Awake() {
        grindNodes = targets.Select(x => (GrindNode)x).ToArray();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if(grindNodes.Count() > 1) {
            for(int i = 1; i < grindNodes.Count(); i++) {
                if(grindNodes[i - 1].Grind != grindNodes[i].Grind) {
                    EditorGUILayout.HelpBox("You have selected nodes from different Grinds. This is probably a mistake.", MessageType.Warning);
                }
            }
        }
        if(grindNodes.Count() == 2) {
            var n0 = grindNodes[0];
            var n1 = grindNodes[1];
            if(n0.Grind == n1.Grind && !n0.IsConnectedTo(n1) && GUILayout.Button("Join nodes")) {
                n0.Grind.AddLine(n0, n1, n0.grindLines.Find(x => x != null));
            }
        }
    }
}