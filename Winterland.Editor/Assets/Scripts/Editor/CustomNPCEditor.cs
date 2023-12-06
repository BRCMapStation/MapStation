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
        var npc = serializedObject.targetObject as CustomNPC;
        var dialogBranches = npc.GetComponents<DialogueBranch>();
        if (dialogBranches.Length <= 0)
            EditorGUILayout.HelpBox("No dialogue branches - Nothing will happen when you interact with this NPC.", MessageType.Warning);
        DrawDefaultInspector();
        EditorGUILayout.Space();
       
        
        
        showBranches = GUILayout.Toggle(showBranches, $"Dialogue Branches ({dialogBranches.Length})", "DropDownButton");
        if (showBranches) {
            EditorGUILayout.HelpBox("Dialogue branch priority is from top to bottom.", MessageType.Info);
            EditorGUI.indentLevel++;
            for(var i=0;i<dialogBranches.Length;i++) {
                var branch = dialogBranches[i];
                GUILayout.BeginVertical($"Branch #{i}", "window");
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
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space();
            if (GUILayout.Button("Add Branch")) {
                var newBranch = npc.gameObject.AddComponent<DialogueBranch>();
                newBranch.hideFlags = HideFlags.HideInInspector;
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.Separator();
        }
        //EditorGUILayout.LabelField(serializedObject.targetObject.GetType().ToString(), EditorStyles.boldLabel);
    }
}
