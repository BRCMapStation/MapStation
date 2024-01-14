using System.Linq;
using Reptile;
using UnityEditor;
using UnityEngine;
using MapStation.Components;

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
        if(!allSameGrind) {
            EditorGUILayout.HelpBox("You have selected nodes from different Grinds. This is probably a mistake.", MessageType.Warning);
        }

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
        if(CanDoAddNodeAction() && GUILayout.Button("Add node")) {
            AddNodeAction();
        }

        if(grindNodes.Length == 2) {
            var n0 = grindNodes[0];
            var n1 = grindNodes[1];
            if(n0.Grind == n1.Grind && !n0.IsConnectedTo(n1) && GUILayout.Button("Join nodes")) {
                n0.Grind.AddLine(n0, n1, n0.grindLines.Find(x => x != null));
            }
        }
    }

    public bool CanDoAddNodeAction() {
        return allSameGrind;
    }

    public void AddNodeAction() {
        grind.AddNodes(grindNodes);
    }
}