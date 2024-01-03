using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using UnityEditor.ShortcutManagement;

[CustomEditor(typeof(BRCMap))]
public class BRCMapEditor : Editor
{
    private BRCMap thisMap;
    private BRCMapBuilder builder;

    private void Awake()
    {
        thisMap = target as BRCMap;
        builder = new BRCMapBuilder(thisMap);
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.HelpBox("You should have your own folder in the Assets folder with the exact same name as your map name entered in the field above." +
            Environment.NewLine +
            Environment.NewLine +
            "All the assets used by your map should be contained in this folder and subfolders. " +
            "Certain file types, such as scenes and scripts, will be ignored as they cannot be used." +
            Environment.NewLine +
            Environment.NewLine +
            "If building your map is successful, it will appear in the Map Files folder.", MessageType.Info);

        if (GUILayout.Button("Toggle Red Debug Shapes"))
        {
            // Apply any changes made to the prefab
            builder.ToggleDebugObjects();
        }

        if (GUILayout.Button("Create MapPrefab from Scene"))
        {
            // Apply any changes made to the prefab
            builder.CreateMapPrefabFromScene();
        }

        if (GUILayout.Button("Collect Map Assets"))
        {
            // Apply any changes made to the prefab
            builder.AddAssetsToBundle();
        }

        if (GUILayout.Button("Build Map File"))
        {

            // Apply any changes made to the prefab
            builder.CreateMapBundle();
        }
        if (GUILayout.Button("Rebuild All"))
        {
            builder.RebuildAll();
        }
    }
}