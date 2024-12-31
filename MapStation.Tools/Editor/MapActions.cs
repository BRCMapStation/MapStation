using MapStation.Common.Gameplay;
using MapStation.Components;
using Reptile;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class MapActions
{
    [MenuItem(UIConstants.menuLabel + "/" + UIConstants.mapactionsSubmenuLabel + "/" + "Sync All Grind Trigger Radiuses to Preference", priority = (int) UIConstants.MenuOrder.MAP_ACTIONS)]
    private static void SyncGrindRadiuses() {
        var grinds = GameObject.FindObjectsOfType<Grind>(true);
        foreach (var grind in grinds) {
            var serialized = new SerializedObject(grind);
            serialized.FindProperty(nameof(Grind.TriggerRadius)).floatValue = Preferences.instance.grinds.defaultGrindTriggerRadius;
            serialized.ApplyModifiedProperties();
        }
    }

    [MenuItem(UIConstants.menuLabel + "/" + UIConstants.mapactionsSubmenuLabel + "/" + "Make All Grinds Unable to Break Combo", priority = (int) UIConstants.MenuOrder.MAP_ACTIONS)]
    private static void MakeAllGrindsNotBreakCombo() {
        var grindPaths = GameObject.FindObjectsOfType<GrindPath>(true);
        foreach (var grindPath in grindPaths) {
            if (!grindPath.gameObject.GetComponent<GrindPath_FixComboBreakingProperty>()) {
                var comp = Undo.AddComponent<GrindPath_FixComboBreakingProperty>(grindPath.gameObject);
                comp.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
            }
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
