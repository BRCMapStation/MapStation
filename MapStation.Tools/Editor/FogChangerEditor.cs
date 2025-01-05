using UnityEngine;
using UnityEditor;
using MapStation.Components;
using MapStation.Tools.Editor;
using MapStation.Common.Runtime;
using cspotcode.UnityGUI;
using System;
using System.Linq;

[CustomEditor(typeof(FogChanger))]
public class FogChangerEditor : Editor {
    public override void OnInspectorGUI() {
        var props = serializedObject.IterChildren();
        if (!RenderSettings.fog) {
            GUI.enabled = false;
            EditorGUILayout.HelpBox("Fog must be enabled in the scene for the Fog Changer to work.", MessageType.Error);
        }
        foreach(var prop in props) {
            if ((prop.name == nameof(FogChanger.Start) || prop.name == nameof(FogChanger.End)) && RenderSettings.fogMode != FogMode.Linear)
                continue;
            if (prop.name == nameof(FogChanger.Density) && RenderSettings.fogMode != FogMode.Exponential && RenderSettings.fogMode != FogMode.ExponentialSquared)
                continue;
            prop.Draw();
        }
        GUI.enabled = true;
        serializedObject.ApplyModifiedProperties();
    }
}
