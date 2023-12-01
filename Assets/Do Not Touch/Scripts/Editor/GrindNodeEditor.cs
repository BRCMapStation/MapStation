using System.Linq;
using Reptile;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GrindNode))]
[CanEditMultipleObjects]
class GrindNodeEditor : Editor
{
    private GrindNode[] grindNodes;
    private bool allSameGrind;
    private Grind grind;
    private void Awake() {
        grindNodes = targets.Select(x => (GrindNode)x).ToArray();
        grind = grindNodes[0]?.Grind;
        allSameGrind = true;
        for(var i = 1; i < grindNodes.Length; i++) {
            if(grindNodes[i].Grind != grind) {
                allSameGrind = false;
                grind = null;
                break;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(GUILayout.Button("Orient upright")) {
            foreach(var grindNode in grindNodes) {
                grindNode.Button_OrientUp();
            }
        }
        if(GUILayout.Button("Orient upside-down")) {
            foreach(var grindNode in grindNodes) {
                grindNode.Button_OrientDown();
            }
        }
        if(allSameGrind == true && GUILayout.Button("Add node")) {
            grind.AddNodes(grindNodes);
        }

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