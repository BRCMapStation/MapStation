using MapStation.Common.Gameplay;
using Reptile;
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GrindPath))]
public class GrindPathEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        var grindSoundOptionNames = new string[] {
            "None",
            "Cable",
            "Metal Hollow",
            "Metal",
            "Metal Solid"
        };
        var grindSoundOptionValues = new SfxCollectionID[] {
            SfxCollectionID.NONE,
            SfxCollectionID.GrindCableSfx,
            SfxCollectionID.GrindMetalHollow,
            SfxCollectionID.GrindMetalSfx,
            SfxCollectionID.GrindMetalSolidSfx
        };
        var sfxProperty = serializedObject.FindProperty(nameof(GrindPath.sfxCollection));
        var currentSFX = Array.IndexOf(grindSoundOptionValues, (SfxCollectionID)sfxProperty.enumValueFlag);
        if (currentSFX == -1)
            currentSFX = 0;
        var newSFX = EditorGUILayout.Popup("Sound Effects", currentSFX, grindSoundOptionNames);
        if (newSFX != currentSFX) {
            sfxProperty.enumValueFlag = (int)grindSoundOptionValues[newSFX];
        }

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

        serializedObject.ApplyModifiedProperties();
    }
}
