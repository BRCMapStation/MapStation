using UnityEngine;
using UnityEditor;
using MapStation.Components;
using MapStation.Tools.Editor;
using MapStation.Common.Runtime;
using cspotcode.UnityGUI;

[CustomEditor(typeof(MapOptions))]
public class MapOptionsEditor : Editor {
    public override void OnInspectorGUI() {
        EditorHelper.MakeDocsButton("Map-Options");
        var options = serializedObject.FindProperty(nameof(MapOptions.Options));
        var size = options.arraySize;
        for(var i = 0; i < size; i++) {
            var elem = options.GetArrayElementAtIndex(i);
            GUILayout.BeginVertical(GUI.skin.box);
            elem.FindPropertyRelative(nameof(MapOptions.MapOption.Name)).Draw();
            elem.FindPropertyRelative(nameof(MapOptions.MapOption.Description)).Draw();
            EditorGUILayout.Separator();
            GUILayout.Label("Possible values");
            var possibleValues = elem.FindPropertyRelative(nameof(MapOptions.MapOption.PossibleValues));
            var possibleValuesCount = possibleValues.arraySize;
            for(var n = 0; n < possibleValuesCount; n++) {
                EditorGUILayout.BeginHorizontal();
                var valueElem = possibleValues.GetArrayElementAtIndex(n);
                valueElem.stringValue = EditorGUILayout.TextField(valueElem.stringValue);
                if (GUILayout.Button("Remove")) {
                    possibleValues.DeleteArrayElementAtIndex(n);
                    serializedObject.ApplyModifiedProperties();
                    return;
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Add")) {
                possibleValues.InsertArrayElementAtIndex(possibleValuesCount);
                serializedObject.ApplyModifiedProperties();
                return;
            }
            GUILayout.Label("Default value");
            var possibleValuesArray = new string[possibleValuesCount + 1];
            possibleValuesArray[0] = "Invalid";
            for(var n = 0; n < possibleValuesCount; n++) {
                possibleValuesArray[n + 1] = possibleValues.GetArrayElementAtIndex(n).stringValue;
            }
            var currentIndex = 0;
            var defValue = elem.FindPropertyRelative(nameof(MapOptions.MapOption.DefaultValue));
            for(var n = 0; n < possibleValuesCount; n++) {
                var val = possibleValues.GetArrayElementAtIndex(n).stringValue;
                if (val == defValue.stringValue) {
                    currentIndex = n + 1;
                    break;
                }
            }
            var newIndex = EditorGUILayout.Popup(currentIndex, possibleValuesArray);
            if (newIndex != currentIndex) {
                defValue.stringValue = possibleValuesArray[newIndex];
            }
            EditorGUILayout.Separator();
            if (GUILayout.Button("Remove")) {
                options.DeleteArrayElementAtIndex(i);
                serializedObject.ApplyModifiedProperties();
                return;
            }
            GUILayout.EndVertical();
        }
        if (GUILayout.Button("Add")) {
            options.InsertArrayElementAtIndex(size);
            var newelem = options.GetArrayElementAtIndex(size);
            newelem.FindPropertyRelative(nameof(MapOptions.MapOption.Name)).stringValue = "New Option";
            newelem.FindPropertyRelative(nameof(MapOptions.MapOption.Description)).stringValue = "";
            var posValuesProp = newelem.FindPropertyRelative(nameof(MapOptions.MapOption.PossibleValues));
            posValuesProp.ClearArray();
            posValuesProp.InsertArrayElementAtIndex(0);
            posValuesProp.GetArrayElementAtIndex(0).stringValue = "ON";
            posValuesProp.InsertArrayElementAtIndex(0);
            posValuesProp.GetArrayElementAtIndex(0).stringValue = "OFF";
            newelem.FindPropertyRelative(nameof(MapOptions.MapOption.DefaultValue)).stringValue = "OFF";
            serializedObject.ApplyModifiedProperties();
            return;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
