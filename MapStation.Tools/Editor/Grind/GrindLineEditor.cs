using System.Linq;
using Reptile;
using UnityEditor;
using UnityEngine;
using MapStation.Components;

[CustomEditor(typeof(GrindLine))]
[CanEditMultipleObjects]
class GrindLineEditor : Editor
{
    private GrindLine[] grindLines;
    private bool allSameGrind;
    private Grind grind;
    private void Awake() {
        grindLines = (from target in targets select (GrindLine)target).ToArray();
        grind = grindLines[0]?.Grind;
        allSameGrind = grind != null;
        for(var i = 1; i < grindLines.Length; i++) {
            if(grindLines[i].Grind != grind) {
                allSameGrind = false;
                grind = null;
                break;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        if(!allSameGrind) {
            EditorGUILayout.HelpBox("You have selected lines from different Grinds. This is probably a mistake.", MessageType.Warning);
        }

        DrawDefaultInspector();

        if(GUILayout.Button("Split Line")) {
            GrindActions.SplitLines(grindLines);
        }
    }
}