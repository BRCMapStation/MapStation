using System.Collections;
using System.Collections.Generic;
using Reptile;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Grind))]
public class GrindEditor : Editor
{
    public Grind thisGrind;

    private void Awake()
    {
        thisGrind = target as Grind;
    }
    public override void OnInspectorGUI()
    {
        thisGrind.ListNodes();
        thisGrind.ListLines();
        DrawDefaultInspector();
        if (GUILayout.Button("Add Node"))
        {
            thisGrind.AddNode();
        }
        if (GUILayout.Button("Remove Node"))
        {
            thisGrind.RemoveNode();
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
