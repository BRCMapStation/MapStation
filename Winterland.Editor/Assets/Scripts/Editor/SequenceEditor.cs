using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Winterland.Common;

[CustomEditor(typeof(Sequence))]
public class SequenceEditor : Editor {
    private bool showActions = false;
    private Type[] actionTypes = new Type[] {
        typeof(DialogSequenceAction),
        typeof(EndSequenceAction),
        typeof(SetActionToRunOnEndAction),
        typeof(SwitchCharacterSequenceAction)
    };
    private Dictionary<SequenceAction, Editor> cachedEditors = new();
    public override void OnInspectorGUI() {
        var sequence = serializedObject.targetObject as Sequence;
        var actions = SequenceAction.GetComponentsOrdered<SequenceAction>(sequence.gameObject);
        if (actions.Length <= 0)
            EditorGUILayout.HelpBox("Sequence doesn't have any actions. That is not okay!", MessageType.Error);
        DrawDefaultInspector();
        if (!string.IsNullOrEmpty(sequence.RunActionOnEnd)) {
            if (sequence.GetActionByName(sequence.RunActionOnEnd) == null)
                EditorGUILayout.HelpBox($"Action {sequence.RunActionOnEnd} doesn't exist!", MessageType.Error);
        }
        EditorGUILayout.Space();

        GUILayout.BeginVertical("window");
        showActions = GUILayout.Toggle(showActions, $"Actions ({actions.Length})", "DropDownButton");
        if (showActions) {
            for (var i = 0; i < actions.Length; i++) {
                EditorGUILayout.Space();
                var action = actions[i];
                var actionName = $"{action.GetType().Name} - Action #{i}";
                if (!string.IsNullOrEmpty(action.Name))
                    actionName = $"{action.Name} - Action #{i}";
                GUILayout.BeginVertical(actionName, "window");
                if (!cachedEditors.TryGetValue(action, out var editor)) {
                    editor = CreateEditor(action);
                }
                else {
                    CreateCachedEditor(action, null, ref editor);
                }
                cachedEditors[action] = editor;
                editor.OnInspectorGUI();
                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Remove")) {
                    DestroyImmediate(action);
                }
                if (GUILayout.Button("Move Up")) {
                    EditorHelper.MoveUp(editor.serializedObject);
                    EditorUtility.SetDirty(editor.serializedObject.targetObject);
                }
                if (GUILayout.Button("Move Down")) {
                    EditorHelper.MoveDown(editor.serializedObject);
                    EditorUtility.SetDirty(editor.serializedObject.targetObject);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            var possibleActions = new string[actionTypes.Length + 1];
            possibleActions[0] = "Add Action";
            for (var i = 0; i < actionTypes.Length; i++) {
                possibleActions[i + 1] = actionTypes[i].Name;
            }
            EditorGUILayout.Space();
            var actionIndex = EditorGUILayout.Popup(0, possibleActions);
            if (actionIndex > 0) {
                var actionToMake = actionTypes[actionIndex - 1];
                sequence.gameObject.AddComponent(actionToMake);
            }
        }
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
    }
}
