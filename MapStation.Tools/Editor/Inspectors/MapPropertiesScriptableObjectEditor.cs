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

    public override void OnInspectorGUI() {

        var assetPath = AssetDatabase.GetAssetPath(properties);
        var map = MapDatabase.GetMapForAssetPath(assetPath);

        if(map != null) map.Properties.SyncAutomaticFields(map);

        serializedObject.Update();

        EditorGUIUtility.labelWidth = 170;
        foreach(var prop in serializedObject.IterChildren()) {
            if(prop.name == nameof(properties.properties)) {
                foreach(var propProp in prop.IterChildren()) {
                    if(propProp.name == nameof(MapProperties.format)) {
                        using(Disabled()) {
                            propProp.Draw();
                        }
                    }
                    else if(propProp.name == nameof(MapProperties.shadowDistance)) {
                        using (Disabled(!properties.properties.overrideShadowDistance)) {
                            propProp.Draw();
                        }
                    }
                    else if(propProp.name == nameof(MapProperties.internalName)) {
                        using(Disabled()) {
                            propProp.Draw();
                        }
                    } else {
                        propProp.Draw();
                    }
                }
            } else if(prop.name == nameof(properties.thunderstoreName)) {
                using(Disabled(properties.setThunderstoreNameAutomatically)) {
                    prop.Draw();
                }
            } else if(prop.name == nameof(properties.setThunderstoreNameAutomatically)) {
                PropertyField(prop, new GUIContent("Auto-set Thunderstore Name"));
            } else {
                PropertyField(prop);
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
