using MapStation.Common.Gameplay;
using Reptile;
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GrindPath))]
[CanEditMultipleObjects]
public class GrindPathEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        // Unused In-Game
        // GrindEditorUtility.MakeGrindSFXDropdown(serializedObject, nameof(GrindPath.sfxCollection));
        var mixed = false;
        var fixComboBreakingSet = false;

        for (var i = 0; i < serializedObject.targetObjects.Length; i++) {

            var previousFixComboBreakingSet = fixComboBreakingSet;

            if (serializedObject.targetObjects[i].GetComponent<GrindPath_FixComboBreakingProperty>() != null) {
                fixComboBreakingSet = true;
            } else {
                fixComboBreakingSet = false;
            }

            if (fixComboBreakingSet != previousFixComboBreakingSet && i > 0) {
                mixed = true;
                fixComboBreakingSet = false;
                break;
            }
        }

        EditorGUI.showMixedValue = mixed;
        var fixComboBreakingToggle = EditorGUILayout.Toggle("Prevent Combo Breaking", fixComboBreakingSet);
        if (fixComboBreakingToggle != fixComboBreakingSet) {
            if (fixComboBreakingToggle) {
                foreach (var targetObject in serializedObject.targetObjects) {
                    if (targetObject.GetComponent<GrindPath_FixComboBreakingProperty>() != null)
                        continue;
                    var comp = Undo.AddComponent<GrindPath_FixComboBreakingProperty>((targetObject as GrindPath).gameObject);
                    comp.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
                }
            } else {
                foreach (var targetObject in serializedObject.targetObjects) {
                    var comp = targetObject.GetComponent<GrindPath_FixComboBreakingProperty>();
                    if (comp != null) {
                        Undo.DestroyObjectImmediate(comp);
                    }
                }
            }
        }
        EditorGUI.showMixedValue = false;

        serializedObject.ApplyModifiedProperties();
    }
}
