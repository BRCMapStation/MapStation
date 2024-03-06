using MapStation.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Lets the user edit the mini-map inside of the map scene context.
/// </summary>
public static class EditMiniMapButton {
    private const string MiniMapInstanceName = "MiniMap - For Editing";
    private static PrefabStage CurrentMiniMapPrefabStage = null;
    private static GameObject CurrentMiniMapPrefabInstance = null;
    [MenuItem(UIConstants.menuLabel + "/Edit MiniMap", priority = (int) UIConstants.MenuOrder.EDIT_MINIMAP)]
    private static void EditMiniMap() {
        var scene = SceneManager.GetActiveScene();
        var sceneFolder = Path.GetDirectoryName(scene.path);
        var miniMapPrefabPath = Path.Combine(sceneFolder, "MiniMap.prefab");
        var miniMapPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(miniMapPrefabPath);
        var miniMapInstance = PrefabUtility.InstantiatePrefab(miniMapPrefab) as GameObject;
        miniMapInstance.name = MiniMapInstanceName;
        var prefabAssetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(miniMapInstance);
        miniMapInstance.hideFlags = HideFlags.DontSave;
        var prefabStage = PrefabStageUtility.OpenPrefab(prefabAssetPath, miniMapInstance, PrefabStage.Mode.InContext);
        CurrentMiniMapPrefabInstance = miniMapInstance;
        CurrentMiniMapPrefabStage = prefabStage;
        PrefabStage.prefabStageClosing -= OnPrefabStageClosing;
        PrefabStage.prefabStageClosing += OnPrefabStageClosing;
    }

    // Delete the minimap instance we placed in the scene once we leave the prefab stage.
    private static void OnPrefabStageClosing(PrefabStage stage) {
        PrefabStage.prefabStageClosing -= OnPrefabStageClosing;
        if (CurrentMiniMapPrefabStage == null || CurrentMiniMapPrefabInstance == null)
            return;
        if (stage != CurrentMiniMapPrefabStage)
            return;
        GameObject.DestroyImmediate(CurrentMiniMapPrefabInstance);
    }

    [MenuItem(UIConstants.menuLabel + "/Edit MiniMap", true, priority = (int) UIConstants.MenuOrder.EDIT_MINIMAP)]
    private static bool EditMiniMap_Verify() {
        var scene = SceneManager.GetActiveScene();
        var sceneFolder = Path.GetDirectoryName(scene.path);
        var miniMapPrefabPath = Path.Combine(sceneFolder, "MiniMap.prefab");
        return File.Exists(miniMapPrefabPath);
    }
}
