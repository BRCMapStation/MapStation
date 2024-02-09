using System.Linq;
using cspotcode.UnityGUI;
using Reptile;
using UnityEditor;
using UnityEngine;
using MapStation.Components;
using static UnityEditor.EditorGUILayout;
using static UnityEditor.EditorGUI;
using static UnityEngine.GUILayout;
using static UnityEngine.GUI;
using static cspotcode.UnityGUI.GUIUtil;

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
        
        // Mimic default inspector, with customization
        serializedObject.Update();
        foreach (var prop in serializedObject.IterChildren(skipScriptName: false)) {
            if (prop.name == nameof(GrindNode.retour)) {
                PropertyField(prop, new GUIContent("Retour / Turn around"));
            } else {
                PropertyField(prop);
            }
        }
        serializedObject.ApplyModifiedProperties();
        
        Space();

        if(GUILayout.Button("Orient Upright")) {
            GrindActions.OrientNodesUpward(grindNodes);
        }
        if(GUILayout.Button("Orient Upside-Down")) {
            GrindActions.OrientNodesDownward(grindNodes);
        }
        if(CanDoAddNodeAction() && GUILayout.Button("Add Node")) {
            AddNodeAction();
        }
        if(CanDoJoinNodesAction() && GUILayout.Button("Connect Nodes")) {
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
