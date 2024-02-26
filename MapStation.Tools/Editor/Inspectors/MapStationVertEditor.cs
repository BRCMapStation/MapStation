using UnityEngine;
using UnityEditor;
using System.Reflection;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using MapStation.Common.Gameplay;
using MapStation.Tools.Editor;

[CustomEditor(typeof(MapStationVert))]
class MapStationVertEditor : Editor {
    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("This is the custom vert component bundled with MapStation.", MessageType.Info);
        EditorHelper.MakeDocsButton("Map-Station-Vert");
        DrawDefaultInspector();
    }
}
