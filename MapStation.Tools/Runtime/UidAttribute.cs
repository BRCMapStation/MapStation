using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class UidAttribute : PropertyAttribute
{}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(UidAttribute))]
public class UidDrawer : PropertyDrawer {
    private const int ButtonWidth = 100;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var fieldPosition = position;
        var buttonPosition = position;
        buttonPosition.x = fieldPosition.xMax = fieldPosition.xMax - ButtonWidth;
        buttonPosition.width = ButtonWidth;
        EditorGUI.PropertyField(fieldPosition, property, label, true);
        if(GUI.Button(buttonPosition, "Generate")) {
            property.stringValue = Guid.NewGuid().ToString();
        }
    }
}
#endif
