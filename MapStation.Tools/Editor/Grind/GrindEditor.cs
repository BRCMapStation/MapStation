using UnityEngine;
using UnityEditor;
using MapStation.Components;
using MapStation.Tools.Editor;

[CustomEditor(typeof(Grind))]
public class GrindEditor : Editor
{
    public new Grind target;

    private void Awake()
    {
        target = base.target as Grind;
    }

    public override void OnInspectorGUI()
    {
        EditorHelper.MakeDocsButton("Grind");
        DrawDefaultInspector();
        if (GUILayout.Button("Repair"))
        {
            GrindActions.Repair(target);
        }
        if (GUILayout.Button("Re-number"))
        {
            GrindActions.Renumber(target);
        }
        if (GUILayout.Button("Add Node"))
        {
            GrindActions.AddNode(target);
        }
        if (GUILayout.Button("Remove Node"))
        {
            GrindActions.RemoveLastNode(target);
        }
    }

    // private void OnSceneGUI()
    // {
    //     // Avoid race condition when grind is deleted yet this still runs (?)
    //     if(thisGrind == null) return;

    //     foreach (GrindNode node in thisGrind.nodes)
    //     {
    //         var oldPos = node.transform.position;
    //         var newPos = Handles.PositionHandle(oldPos, Quaternion.identity);

    //         if(newPos != oldPos)
    //         {
    //             // Record the object's state for undo
    //             Undo.RecordObject(node.transform, "Move Grind Node");

    //             node.transform.position = newPos;
    //         }
    //     }
    // }
}
