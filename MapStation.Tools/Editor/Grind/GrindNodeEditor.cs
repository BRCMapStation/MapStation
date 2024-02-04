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
        grindNodes = (from target in targets select (GrindNode)target).ToArray();
        grind = grindNodes[0]?.Grind;
        allSameGrind = grind != null;
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

        if(GUILayout.Button("Orient Upright")) {
            GrindActions.OrientNodesUpward(grindNodes);
        }
        if(GUILayout.Button("Orient Upside-Down")) {
            GrindActions.OrientNodesDownward(grindNodes);
        }
        if(CanDoAddNodeAction() && GUILayout.Button("Add Node")) {
            AddNodeAction();
        }
        if(CanDoJoinNodesAction() && GUILayout.Button("Join Nodes")) {
            JoinNodesAction();
        }
    }

    public bool CanDoAddNodeAction() {
        return allSameGrind;
    }

    public void AddNodeAction() {
        GrindActions.AddNodes(grind, grindNodes);
    }

    public bool CanDoJoinNodesAction() {
        if(grindNodes.Length == 2) {
            var n0 = grindNodes[0];
            var n1 = grindNodes[1];
            return GrindActions.CanJoinNodes(n0, n1);
        }
        return false;
    }

    public void JoinNodesAction() {
        var n0 = grindNodes[0];
        var n1 = grindNodes[1];
        GrindActions.JoinNodes(n0, n1);
    }
}