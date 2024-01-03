using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// Like [HideInInspector], but it only hides the property, still shows any
/// [Header] or other Drawers attached to it.

[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class HideValueInInspectorAttribute : PropertyAttribute
{
    public HideValueInInspectorAttribute() {}
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(HideValueInInspectorAttribute))]
public class HideValueInInspectorDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 0;
    }
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label) {}
}
#endif