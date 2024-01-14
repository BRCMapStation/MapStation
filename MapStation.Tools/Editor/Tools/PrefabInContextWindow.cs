using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PrefabInContextWindow : EditorWindow {
    const string WindowLabel = "Open Prefab In Context";

    [MenuItem(UIConstants.menuLabel + "/" + UIConstants.experimentsSubmenuLabel + "/" + WindowLabel, priority = (int)UIConstants.MenuOrder.EXPERIMENTS)]
    private static void ShowMyEditor() {
        EditorWindow wnd = GetWindow<PrefabInContextWindow>();
        wnd.titleContent = new GUIContent(WindowLabel);
    }

    PrefabInContextWindow() : base() {
        var minSize = this.minSize = new Vector2(100, 40);
        var type = typeof(Editor).Assembly.GetType("UnityEditor.HostView");
        var fieldInfo = type.GetField("k_DockedMinSize", BindingFlags.Static | BindingFlags.NonPublic );
        fieldInfo!.SetValue( null, minSize );
    }

    private string objectName = "";

    private void OnGUI() {
        objectName = EditorGUILayout.TextField(objectName);
        if(GUILayout.Button("Open")) {
            var gameObject = GameObject.Find(objectName);
            var prefabAssetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
            PrefabStageUtility.OpenPrefab(prefabAssetPath, gameObject, PrefabStage.Mode.InContext);
        }
    }
}