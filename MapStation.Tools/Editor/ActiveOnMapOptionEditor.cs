using UnityEngine;
using UnityEditor;
using MapStation.Components;
using MapStation.Tools.Editor;
using MapStation.Common.Runtime;
using cspotcode.UnityGUI;
using System;
using System.Linq;

[CustomEditor(typeof(ActiveOnMapOption))]
public class ActiveOnMapOptionEditor : Editor {
    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("Conditionally activate this GameObject depending on a Map Option.", MessageType.Info);
        var isInvalid = false;
        EditorHelper.MakeDocsButton("Map-Options");
        EditorGUILayout.Separator();
        var mapOptions = MapOptions.Instance;
        var hasMapOptions = false;
        if (mapOptions != null) {
            if (mapOptions.Options.Length > 0)
                hasMapOptions = true;
        }
        if (!hasMapOptions) {
            EditorGUILayout.HelpBox("No Map Options have been defined in this map.", MessageType.Error);
            return;
        }
        var filterModeProp = serializedObject.FindProperty(nameof(ActiveOnMapOption.FilterMode));
        filterModeProp.Draw();
        var optionProp = serializedObject.FindProperty(nameof(ActiveOnMapOption.OptionName));
        var valuesProp = serializedObject.FindProperty(nameof(ActiveOnMapOption.OptionValues));
        var possibleOptions = new string[mapOptions.Options.Length + 1];
        possibleOptions[0] = "INVALID";
        for(var i = 0; i < mapOptions.Options.Length; i++) {
            possibleOptions[i + 1] = mapOptions.Options[i].Name;
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Option");
        var currentIndex = Array.IndexOf(possibleOptions, optionProp.stringValue);
        if (currentIndex <= 0) {
            currentIndex = 0;
            isInvalid = true;
        }
        var newIndex = EditorGUILayout.Popup(currentIndex, possibleOptions);
        if (newIndex != currentIndex) {
            optionProp.stringValue = possibleOptions[newIndex];
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("Values");
        var valuesSize = valuesProp.arraySize;
        var mapOption = GetMapOption(optionProp.stringValue);
        if (mapOption != null) {
            for (var i = 0; i < valuesSize; i++) {
                EditorGUILayout.BeginHorizontal("box");
                var str = valuesProp.GetArrayElementAtIndex(i).stringValue;
                if (!mapOption.PossibleValues.Contains(str)) {
                    str = "INVALID";
                    isInvalid = true;
                }
                EditorGUILayout.LabelField(str);
                if (GUILayout.Button("Remove")) {
                    valuesProp.DeleteArrayElementAtIndex(i);
                    serializedObject.ApplyModifiedProperties();
                    return;
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Add")) {
                var menu = new GenericMenu();
                if (mapOption == null) return;
                foreach (var possibleValue in mapOption.PossibleValues) {
                    menu.AddItem(new GUIContent(possibleValue), false, (object selected) => {
                        valuesProp.InsertArrayElementAtIndex(valuesSize);
                        var newElem = valuesProp.GetArrayElementAtIndex(valuesSize);
                        newElem.stringValue = possibleValue;
                        serializedObject.ApplyModifiedProperties();
                    }, null);
                }
                menu.ShowAsContext();
            }
        }
        EditorGUILayout.EndVertical();
        if (isInvalid)
            EditorGUILayout.HelpBox("There are one or more invalid values.", MessageType.Warning);
        serializedObject.ApplyModifiedProperties();
    }

    private MapOptions.MapOption GetMapOption(string name) {
        var mapOptions = MapOptions.Instance;
        foreach(var option in mapOptions.Options) {
            if (option.Name == name) return option;
        }
        return null;
    }
}
