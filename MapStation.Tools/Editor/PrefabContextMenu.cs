using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
public class AddPrefabsToContextMenu {
    private const int Priority = -30;
    private const string PrefabPathPrefix = "Packages/com.brcmapstation.tools/Assets/MapComponents/";

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Spawners/Respawn Point", priority = Priority)]
    private static void CreateRespawnPoint(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Player Respawner");
    }

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Spawners/Spawn Point", priority = Priority)]
    private static void CreateSpawnPoint(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Player Spawn Point");
    }

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Teleports/Door Teleport", priority = Priority)]
    private static void CreateTeleport(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "DoorTeleporterPrefab");
    }
    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Teleports/Out-of-Bounds Teleport", priority = Priority)]
    private static void CreateOobTeleport(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "TeleporterPrefab");
    }

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Graffiti/Small Graffiti Spot", priority = Priority)]
    private static void CreateGraffitiSpotSmall(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "GraffitiSpotSmallPrefab");
    }
    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Graffiti/Medium Graffiti Spot", priority = Priority)]
    private static void CreateGraffitiSpotMedium(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "GraffitiSpotMediumPrefab");
    }
    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Graffiti/Large Graffiti Spot", priority = Priority)]
    private static void CreateGraffitiSpotLarge(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "GraffitiSpotLargePrefab");
    }
    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Graffiti/Extra Large Graffiti Spot", priority = Priority)]
    private static void CreateGraffitiSpotExtraLarge(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "GraffitiSpotExtraLargePrefab");
    }

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Grind", priority = Priority)]
    private static void CreateGrind(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Grind");
    }

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Vert Ramp", priority = Priority)]
    private static void CreateVertRamp(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "VertRampPrefab");
    }
    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Wallrun", priority = Priority)]
    private static void CreateWallrun(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "WallRunPrefab");
    }
    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Vending Machine", priority = Priority)]
    private static void CreateVendingMachine(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "VendingMachinePrefab");
    }
    
    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Skateboard Screw Pole", priority = Priority)]
    private static void CreateSkateboardScrewPole(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Skateboard Screw Pole");
    }
    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Cypher", priority = Priority)]
    private static void CreateCypher(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Cypher");
    }
    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Sun", priority = Priority)]
    private static void CreateSun(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Sun");
    }

    private static void CreatePrefabUnderContext(Object context, string PrefabName, bool supportUndo = true) {
        var assetPath = PrefabPathPrefix + PrefabName + ".prefab";
        var prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        if(prefabAsset == null) {
            Debug.LogError(string.Format("Prefab not found at path {0}", assetPath));
            return;
        }
        var prefabInstance = PrefabUtility.InstantiatePrefab(prefabAsset) as GameObject;
        StageUtility.PlaceGameObjectInCurrentStage(prefabInstance);
        GameObjectUtility.SetParentAndAlign(prefabInstance, context as GameObject);
        prefabInstance.transform.position = SceneView.lastActiveSceneView.pivot;
        // Undo for some prefabs is *crashing* Unity Editor.
        // Broken/misbehaving Undo is better than losing your work.
        if(supportUndo) {
            var undoTitle = $"Create {prefabInstance.name}";
            Undo.RegisterCreatedObjectUndo(prefabInstance, undoTitle);
            Undo.RegisterFullObjectHierarchyUndo(prefabInstance, undoTitle);
        }
        Selection.activeObject = prefabInstance;
    }
}
