using MapStation.Common.Gameplay;
using Reptile;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GrindPath))]
public class GrindPathEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        var isFixComboBreakingSet = serializedObject.targetObject.GetComponent<GrindPath_FixComboBreakingProperty>() != null;
        var fixComboBreakingToggle = EditorGUILayout.Toggle("Prevent Combo Breaking", isFixComboBreakingSet);
        if (fixComboBreakingToggle != isFixComboBreakingSet) {
            if (fixComboBreakingToggle) {
                var comp = serializedObject.targetObject.AddComponent<GrindPath_FixComboBreakingProperty>();
                comp.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
            } else {
                DestroyImmediate(serializedObject.targetObject.GetComponent<GrindPath_FixComboBreakingProperty>());
            }
        }
    }
}
