using UnityEngine;
using UnityEditor;
using static UnityEditor.EditorGUILayout;
using static UnityEditor.EditorGUI;
using static UnityEngine.GUILayout;
using static UnityEngine.GUI;
using static cspotcode.UnityGUI.GUIUtil;
using MapStation.Tools;
using MapStation.Common;

[CustomEditor(typeof(MapPropertiesScriptableObject))]
class MapPropertiesScriptableObjectEditor : Editor {
    private MapPropertiesScriptableObject properties => target as MapPropertiesScriptableObject;

    Vector2 scrollPosition;

    public override void OnInspectorGUI() {
        serializedObject.Update();

        var assetPath = AssetDatabase.GetAssetPath(properties);
        var internalName = MapDatabase.GetMapForAssetPath(assetPath).Name;

        EditorGUIUtility.labelWidth = 170;
        foreach(var pref in serializedObject.FindProperty(nameof(properties.properties)).IterChildren()) {
            if(pref.name == nameof(MapProperties.format)) {
                using(Disabled()) {
                    pref.Draw();
                }
            }
            else if(pref.name == nameof(MapProperties.internalName)) {
                pref.stringValue = internalName;
                using(Disabled()) {
                    pref.Draw();
                }
            } else {
                pref.Draw();
            }
        }
    }
}