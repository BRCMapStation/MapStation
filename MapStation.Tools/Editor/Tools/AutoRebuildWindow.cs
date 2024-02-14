using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class AutoRebuildWindow : EditorWindow {
    const string windowLabel = "Auto-Rebuild";

    [MenuItem(UIConstants.menuLabel + "/" + UIConstants.experimentsSubmenuLabel + "/" + windowLabel, priority = (int)UIConstants.MenuOrder.EXPERIMENTS)]
    private static void ShowMyEditor() {
        EditorWindow wnd = GetWindow<AutoRebuildWindow>();
        wnd.titleContent = new GUIContent(windowLabel);
    }

    private Vector2 scrollbarPosition = Vector2.zero;

    private EditorCoroutine routine;

    private void OnEnable() {
        routine = this.StartCoroutine(Worker());
    }
    private void OnDisable() {
        if (routine != null) {
            this.StopCoroutine(routine);
        }
    }

    private IEnumerator<object> Worker() {
        while (true) {
            yield return new EditorWaitForSeconds(1);
            Debug.Log("refreshing");
            // AssetDatabase.Refresh();
            AssetDatabase.ImportAsset("Maps/cspotcode.demo/cube.blend");
            Debug.Log("refreshed");
            MapBuilder.BuildAssets();
        }
    }

    private void OnGUI() {
        GUILayout.Label("Automatically refreshes the asset database even when not focused.");
    }
}
