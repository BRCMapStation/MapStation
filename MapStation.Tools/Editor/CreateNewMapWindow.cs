using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MapStation.Common;
using MapStation.Tools;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using static UnityEditor.EditorGUILayout;
using static UnityEditor.EditorGUI;
using static UnityEngine.GUILayout;
using static UnityEngine.GUI;
using static cspotcode.UnityGUI.GUIUtil;

public class CreateNewMapWindow : EditorWindow {

    const string WindowLabel = "Create Map";

    [MenuItem(UIConstants.menuLabel + "/" + WindowLabel, priority = (int)UIConstants.MenuOrder.CREATE_MAP)]
    private static void ShowWindow() {
        // This method is called when the user selects the menu item in the Editor
        EditorWindow wnd = GetWindow<CreateNewMapWindow>();
        wnd.titleContent = new GUIContent(WindowLabel);

        // Limit size of the window
        wnd.minSize = new Vector2(450, 50);
        wnd.maxSize = new Vector2(1920, 720);

        wnd.ShowModalUtility();
    }

    private string mapName = "authorname.mapname";

    private void OnGUI() {
        Space();
        LabelField("Map name");
        mapName = EditorGUILayout.TextField(mapName);
        Space();
        if(Button("Create Map")) {
            this.Close();
            CreateNewMap(mapName);
        }
    }

    private static void CreateNewMap(string mapName) {
        var srcDir = ToolAssetConstants.NewMapTemplatePath;
        var outDir = AssetNames.GetAssetDirectoryForMap(mapName);

        var srcScenePath = ToolAssetConstants.NewMapTemplateScenePath;
        var desiredScenePath = AssetNames.GetScenePathForMap(mapName);

        string[] guids = AssetDatabase.FindAssets("", new[] { srcDir });
        foreach (var guid in guids) {
            var srcPath = AssetDatabase.GUIDToAssetPath(guid);
            var outPath = srcPath.Replace(srcDir, outDir);

            // Scene should be named correctly; everything else can be copied with same filename
            if(srcPath == srcScenePath) outPath = desiredScenePath;

            // Throw exception to avoid overwriting things.
            // Also if path renaming fails, cuz that indicates a logic bug
            if(outPath == srcPath || AssetDatabase.GetMainAssetTypeAtPath(outPath) != null) {
                throw new Exception($"this should never happen: {srcPath} -> {outPath} {AssetDatabase.GUIDFromAssetPath(outPath)}");
            }

            // Create parent directory
            var d = Path.GetDirectoryName(outPath);
            if(!AssetDatabase.IsValidFolder(d)) {
                AssetDatabase.CreateFolder(Path.GetDirectoryName(d), Path.GetFileName(d));
            }
            // Copy asset
            AssetDatabase.CopyAsset(srcPath, outPath);
        }
        AssetDatabase.SaveAssets();

        var map = MapDatabase.GetMap(mapName);
        var maps = new [] {map};
        MapBuilder.StubMissingMapFiles(maps);
        AssetDatabase.SaveAssets();
        MapBuilder.SyncMapProperties(maps);
        AssetDatabase.SaveAssets();

        // Open the new map
        EditorSceneManager.OpenScene(map.ScenePath, OpenSceneMode.Single);
    }
}
