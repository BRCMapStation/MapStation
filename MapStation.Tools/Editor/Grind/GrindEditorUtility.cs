using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

public static class GrindEditorUtility {
    public static void MakeGrindSFXDropdown(SerializedObject serializedObject, string propertyName) {
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
        var sfxProperty = serializedObject.FindProperty(propertyName);
        var currentSFX = Array.IndexOf(grindSoundOptionValues, (SfxCollectionID) sfxProperty.enumValueFlag);
        if (currentSFX == -1)
            currentSFX = 0;
        EditorGUI.showMixedValue = sfxProperty.hasMultipleDifferentValues;
        if (sfxProperty.hasMultipleDifferentValues)
            currentSFX = -1;
        var newSFX = EditorGUILayout.Popup("Sound Effects", currentSFX, grindSoundOptionNames);
        EditorGUI.showMixedValue = false;
        if (newSFX != currentSFX) {
            sfxProperty.enumValueFlag = (int) grindSoundOptionValues[newSFX];
        }
    }
}
