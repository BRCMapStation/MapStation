using System;
using HarmonyLib;
using Reptile;
using UnityEngine;

namespace MapStation.Plugin.Patches;

[HarmonyPatch(typeof(StageManager))]
public class StageManagerPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(StageManager.SetupStage))]
    private static void SetupStage_Prefix(StageManager __instance) {
        // Create a default spawner if the map has no spawners
        if (GameObject.FindObjectOfType<PlayerSpawner>() == null) {
            var go = new GameObject();
            var spawner = go.AddComponent<PlayerSpawner>();
            spawner.isDefaultSpawnPoint = true;
        }
        
        // Create a SunFlareGPU if the map doesn't have one
        if (GameObject.FindObjectOfType<SunFlareGPU>() == null) {
            var go = new GameObject();
            var sunflareGpu = go.AddComponent<SunFlareGPU>();
            var occlusionCamera = new GameObject();
            occlusionCamera.transform.SetParent(go.transform);
            occlusionCamera.AddComponent<Camera>();
        }
    }
}
