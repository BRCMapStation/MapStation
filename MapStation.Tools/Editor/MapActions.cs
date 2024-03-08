using MapStation.Common.Gameplay;
using Reptile;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class MapActions
{
    [MenuItem(UIConstants.menuLabel + "/" + UIConstants.mapactionsSubmenuLabel + "/" + "Make All Grinds Unable to Break Combo", priority = (int) UIConstants.MenuOrder.MAP_ACTIONS)]
    private static void MakeAllGrindsNotBreakCombo() {
        var grindPaths = GameObject.FindObjectsOfType<GrindPath>(true);
        foreach (var grindPath in grindPaths) {
            if (!grindPath.gameObject.GetComponent<GrindPath_FixComboBreakingProperty>())
                Undo.AddComponent<GrindPath_FixComboBreakingProperty>(grindPath.gameObject);
        }
    }

    [MenuItem(UIConstants.menuLabel + "/" + UIConstants.mapactionsSubmenuLabel + "/" + "Make All Grinds Able to Break Combo", priority = (int) UIConstants.MenuOrder.MAP_ACTIONS)]
    private static void MakeAllGrindsBreakCombo() {
        var grindPaths = GameObject.FindObjectsOfType<GrindPath>(true);
        foreach (var grindPath in grindPaths) {
            var comp = grindPath.gameObject.GetComponent<GrindPath_FixComboBreakingProperty>();
            if (comp != null)
                Undo.DestroyObjectImmediate(comp);
        }
    }
}
