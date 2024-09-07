using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
public class AddPrefabsToContextMenu {

    private enum UnpackMode {
        DontUnpack,
        UnpackRoot,
        UnpackCompletely
    }
    private const int Priority = -30;
    private const string PrefabPathPrefix = "Packages/com.brcmapstation.tools/Assets/MapComponents/";

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Audio/Audio Source", priority = Priority)]
    private static void CreateAudioSource(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Audio Source", true, UnpackMode.UnpackCompletely);
    }

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
        CreatePrefabUnderContext(menuCommand.context, "DoorTeleporter");
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
    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Ambient Trigger", priority = Priority)]
    private static void CreateAmbientTrigger(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Ambient Trigger");
    }
    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Launchers/Launcher", priority = Priority)]
    private static void CreateLauncher(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Launcher");
    }
    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Launchers/Super Launcher", priority = Priority)]
    private static void CreateSuperLauncher(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "SuperLauncher");
    }
    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Speed Zone", priority = Priority)]
    private static void CreateSpeedZone(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "SpeedZone");
    }
    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Walk Zone", priority = Priority)]
    private static void CreateWalkZone(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "WalkZone");
    }
    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Camera Zone", priority = Priority)]
    private static void CreateCameraZone(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Camera Zone");
    }
    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Glass/Blue 8x4", priority = Priority)]
    private static void CreateGlassBlue8x4(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "GlassBlue_8x4");
    }

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Stage Chunk", priority = Priority)]
    private static void CreateStageChunk(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Stage Chunk", true, UnpackMode.UnpackCompletely);
    }

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Props and Junk/Cube Junk", priority = Priority)]
    private static void CreateCubeProp(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Cube Junk");
    }

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Street Life/Pedestrians/Business 1", priority = Priority)]
    private static void CreatePedBusiness1(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Peds/Ped Business 1");
    }

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Street Life/Pedestrians/Business 2", priority = Priority)]
    private static void CreatePedBusiness2(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Peds/Ped Business 2");
    }

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Street Life/Pedestrians/Bun 2", priority = Priority)]
    private static void CreatePedBun2(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Peds/Ped Bun 2");
    }

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Street Life/Pedestrians/Cap 2", priority = Priority)]
    private static void CreatePedCap2(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Peds/Ped Cap 2");
    }

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Street Life/Pedestrians/Lank 1", priority = Priority)]
    private static void CreatePedLank1(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Peds/Ped Lank 1");
    }

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Street Life/Pedestrians/Plus 2", priority = Priority)]
    private static void CreatePedPlus2(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Peds/Ped Plus 2");
    }

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Street Life/Pedestrians/Racer 1", priority = Priority)]
    private static void CreatePedRacer1(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Peds/Ped Racer 1");
    }

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Street Life/Pedestrians/Racer 2", priority = Priority)]
    private static void CreatePedRacer2(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Peds/Ped Racer 2");
    }

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Street Life/Pedestrians/Walking", priority = Priority)]
    private static void CreatePedWalking(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Peds/Walking Ped", true, UnpackMode.UnpackRoot);
    }

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Street Life/Cluster", priority = Priority)]
    private static void CreateStreetLifeCluster(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Street Life Cluster", true, UnpackMode.UnpackRoot);
    }

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/Public Toilet", priority = Priority)]
    private static void CreatePublicToilet(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "Public Toilet", true);
    }

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/NPC/Inline MoveStyle Changer", priority = Priority)]
    private static void CreateInlineNPC(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "NPC/NPC Inline MoveStyle Changer", true);
    }

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/NPC/Skateboard MoveStyle Changer", priority = Priority)]
    private static void CreateSkateboardNPC(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "NPC/NPC Skateboard MoveStyle Changer", true);
    }

    [MenuItem("GameObject/" + UIConstants.menuLabel + "/NPC/BMX MoveStyle Changer", priority = Priority)]
    private static void CreateBMXNPC(MenuCommand menuCommand) {
        CreatePrefabUnderContext(menuCommand.context, "NPC/NPC BMX MoveStyle Changer", true);
    }

    private static void CreatePrefabUnderContext(Object context, string PrefabName, bool supportUndo = true, UnpackMode unpackMode = UnpackMode.DontUnpack) {
        var assetPath = PrefabPathPrefix + PrefabName + ".prefab";
        var prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        if(prefabAsset == null) {
            Debug.LogError(string.Format("Prefab not found at path {0}", assetPath));
            return;
        }
        var parent = context is GameObject go ? go.transform : null;
        var prefabInstance = PrefabUtility.InstantiatePrefab(prefabAsset, parent) as GameObject;
        // Cannot use SetParentAndAlign b/c it changes child's layer to match parent
        prefabInstance.transform.position = SceneView.lastActiveSceneView.pivot;
        // Undo for some prefabs is *crashing* Unity Editor.
        // Broken/misbehaving Undo is better than losing your work.
        if(supportUndo) {
            var undoTitle = $"Create {prefabInstance.name}";
            Undo.RegisterCreatedObjectUndo(prefabInstance, undoTitle);
            Undo.RegisterFullObjectHierarchyUndo(prefabInstance, undoTitle);
        }
        if (unpackMode != UnpackMode.DontUnpack) {
            var parsedUnpackMode = PrefabUnpackMode.Completely;
            if (unpackMode == UnpackMode.UnpackRoot)
                parsedUnpackMode = PrefabUnpackMode.OutermostRoot;
            PrefabUtility.UnpackPrefabInstance(prefabInstance, parsedUnpackMode, InteractionMode.AutomatedAction);
        }
        Selection.activeObject = prefabInstance;
    }
}
