
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
    private static void LoadSceneAsync_Prefix(ref string sceneName, LoadSceneMode __1) {
        // HACK WIP
        // Vanilla BRC assumes that Stage enums match Scene names.
        // So it tries to do like `SceneManager.LoadSceneAsync(currentStage.ToString())`
        // 
        // We need to rewrite names from Stage.ToString(), such as `mapstation/cspotcode.deatheggzone`,
        // into valid Scene names which will match what comes out of Unity Editor, such as `Assets/Maps/cspotcode.deatheggzone/Scene` (scene name? path? I get confused)

        if(SceneNameMapper.Instance.Mappings.TryGetValue(sceneName, out var replacement)) {
            Debug.Log($"{nameof(SceneManager)}.{nameof(SceneManager.LoadSceneAsync)} redirected from {sceneName} to {replacement}");
            sceneName = replacement;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(SceneManager.UnloadSceneAsync))]
    [HarmonyPatch(new Type[] {typeof(string)})]
    private static void UnloadSceneAsync_Prefix(ref string sceneName) {
        // Same logic as LoadSceneAsync patch
        if(SceneNameMapper.Instance.Mappings.TryGetValue(sceneName, out var replacement)) {
            Debug.Log($"{nameof(SceneManager)}.{nameof(SceneManager.UnloadSceneAsync)} redirected from {sceneName} to {replacement}");
            sceneName = replacement;
        }
    }
}
