using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Winterland.Common;
public class ToyEditor {
    [MenuItem("GameObject/Winterland/Create Toy Line", priority = -50)]
    private static void CreateToyLine(MenuCommand menuCommand) {
        var toyLine = CreateToyLineUnderContext(menuCommand.context);
        Undo.RegisterCreatedObjectUndo(toyLine, $"Create {toyLine.name}");
        Selection.activeObject = toyLine.gameObject;
    }

    [MenuItem("GameObject/Winterland/Create Toy Part", priority = -45)]
    private static void CreateToyPart(MenuCommand menuCommand) {
        var selection = Selection.activeTransform;
        ToyLine toyLine = null;
        if (selection != null) {
            toyLine = selection.GetComponentInParent<ToyLine>();
        }
        var createdToyLine = false;
        if (toyLine == null) {
            createdToyLine = true;
            toyLine = CreateToyLineUnderContext(menuCommand.context);
        }
        var toyPartPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Toy Part.prefab");
        var toyPart = PrefabUtility.InstantiatePrefab(toyPartPrefab) as GameObject;
        StageUtility.PlaceGameObjectInCurrentStage(toyPart);
        toyPart.transform.SetParent(toyLine.transform);
        if (createdToyLine)
            Undo.RegisterCreatedObjectUndo(toyLine.gameObject, $"Create {toyPart.name}");
        else
            Undo.RegisterCreatedObjectUndo(toyPart, $"Create {toyPart.name}");
        GameObjectUtility.SetParentAndAlign(toyPart, menuCommand.context as GameObject);
        Selection.activeObject = toyPart;
        toyPart.transform.position = SceneView.lastActiveSceneView.pivot;
        var toyPartComponent = toyPart.GetComponent<ToyPart>();
        if (toyPartComponent != null)
            ToyPartEditor.Randomize(toyPartComponent);
    }

    private static ToyLine CreateToyLineUnderContext(Object context) {
        var toyLine = new GameObject("Toy Line");
        StageUtility.PlaceGameObjectInCurrentStage(toyLine);
        GameObjectUtility.SetParentAndAlign(toyLine, context as GameObject);
        toyLine.transform.position = SceneView.lastActiveSceneView.pivot;
        return toyLine.AddComponent<ToyLine>();
    }
}
