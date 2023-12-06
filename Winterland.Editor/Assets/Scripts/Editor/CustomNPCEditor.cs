using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Winterland.Common;
using UnityEngine.UIElements;
using UnityEditorInternal;

[CustomEditor(typeof(CustomNPC))]
public class CustomNPCEditor : Editor {
    private bool showBranches = false;
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        EditorGUILayout.Space();
       
        var npc = serializedObject.targetObject as CustomNPC;
        var dialogBranches = npc.GetComponents<DialogueBranch>();
        showBranches = GUILayout.Toggle(showBranches, "Dialogue Branches", "DropDownButton");
        if (showBranches) {
            EditorGUI.indentLevel++;
            foreach (var branch in dialogBranches) {
                EditorGUILayout.Separator();
                var editor = Editor.CreateEditor(branch);

                editor.OnInspectorGUI();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Remove")) {
                    DestroyImmediate(branch);
                }
                if (GUILayout.Button("Move Up")) {
                    ComponentUtility.MoveComponentUp(branch);
                }
                if (GUILayout.Button("Move Down")) {
                    ComponentUtility.MoveComponentDown(branch);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Separator();
            }
            if (GUILayout.Button("Add")) {
                var newBranch = npc.gameObject.AddComponent<DialogueBranch>();
                newBranch.hideFlags = HideFlags.HideInInspector;
            }
            EditorGUI.indentLevel--;
        }
        //EditorGUILayout.LabelField(serializedObject.targetObject.GetType().ToString(), EditorStyles.boldLabel);
    }
}
