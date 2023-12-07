using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using Winterland.Common;

[CustomEditor(typeof(DialogSequenceAction))]
public class DialogSequenceActionEditor : Editor {
    private bool showDialogs = false;
    public override void OnInspectorGUI() {
        var action = serializedObject.targetObject as DialogSequenceAction;
        var dialogs = action.gameObject.GetComponents<DialogBlock>().Where((block) => (block.Owner == action)).ToArray();
        if (dialogs.Length <= 0)
            EditorGUILayout.HelpBox("Dialogue doesn't have any dialogues. That doesn't make any sense!", MessageType.Error);

        DrawDefaultInspector();

        if (action.Type == DialogSequenceAction.DialogType.YesNah) {
            var noProp = serializedObject.FindProperty("NahTarget");
            var yesProp = serializedObject.FindProperty("YesTarget");
            var newYesTarget = EditorGUILayout.PropertyField(yesProp);
            var newNahTarget = EditorGUILayout.PropertyField(noProp);
        }
        EditorGUILayout.Space();
        GUILayout.BeginVertical("window");
        showDialogs = GUILayout.Toggle(showDialogs, $"Dialogues ({dialogs.Length})", "DropDownButton");
        if (showDialogs) {
            
            for (var i = 0; i < dialogs.Length; i++) {
                EditorGUILayout.Space();
                var dialog = dialogs[i];
                GUILayout.BeginVertical($"Dialogue #{i}", "window");
                var editor = Editor.CreateEditor(dialog);
                editor.OnInspectorGUI();
                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Remove")) {
                    DestroyImmediate(dialog);
                }
                if (GUILayout.Button("Move Up")) {
                    ComponentUtility.MoveComponentUp(dialog);
                }
                if (GUILayout.Button("Move Down")) {
                    ComponentUtility.MoveComponentDown(dialog);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space();
            if (GUILayout.Button("Add Dialogue")) {
                var newDialogue = action.gameObject.AddComponent<DialogBlock>();
                newDialogue.Owner = action;
                newDialogue.hideFlags = HideFlags.HideInInspector;
            }
        }
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
    }
}
