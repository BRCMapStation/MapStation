
using HarmonyLib;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using MapStation.Plugin;

[HarmonyPatch(typeof(SceneManager))]
internal static class SceneManagerPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(SceneManager.LoadSceneAsync))]
    [HarmonyPatch(new Type[] {typeof(string), typeof(LoadSceneMode)})]
    private static void LoadSceneAsync_Prefix(ref string sceneName) {
        // HACK WIP
        // Vanilla BRC assumes that Stage enums match Scene names.
        // So it tries to do like `SceneManager.LoadSceneAsync(currentStage.ToString())`
        // 
        // We need to rewrite names from Stage.ToString(), such as `mapstation/cspotcode.deatheggzone`,
        // into valid Scene names which will match what comes out of Unity Editor, such as `Assets/Maps/cspotcode.deatheggzone/Scene` (scene name? path? I get confused)
        if(sceneName.StartsWith(StageEnum.MapNamePrefix)) {
            sceneName = "Assets/Maps/DeathEggZone/Scene.unity"; // Should this be a scene "path" instead of a "name"?
        }
    }
}
