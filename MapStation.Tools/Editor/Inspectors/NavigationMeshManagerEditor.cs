using MapStation.Tools.Runtime;
using MapStation.Tools.Editor;
using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NavigationMeshManager))]
[CanEditMultipleObjects]
public class NavigationMeshManagerEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Navigation-Mesh");
        DrawDefaultInspector();
        var navManager = target as NavigationMeshManager;
        if (GUILayout.Button("Generate Navigation Meshes")) {
            navManager.GenerateNavMeshes();
        }
        if (!navManager.CanAlignCopterSpawners())
            GUI.enabled = false;
        if (GUILayout.Button("Align Helicopter Spawns")) {
            navManager.AlignCopterSpawners();
        }
        GUI.enabled = true;
    }
}
