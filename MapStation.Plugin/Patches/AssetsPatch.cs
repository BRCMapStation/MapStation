using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Reptile;
using UnityEngine;
using MapStation.Common;
using System.Runtime.InteropServices;

namespace MapStation.Plugin.Patches;

[HarmonyPatch(typeof(Assets))]
internal static class AssetsPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Assets.Init))]
    private static void Init_Postfix(Assets __instance) {

        // TEST HACK Register a custom map for testing.
        // REMOVE THIS once the idea is proven
        var stageId = StageEnum.ClaimCustomMapId();
        StageEnum.AddMapName(stageId, BootstrapPatch.mapInternalName);

        var collections = __instance.assetBundleLibrary.collections.ToList();
        var collection = ScriptableObject.CreateInstance<AssetBundleCollection>();
        collection.assetBundleCollectionName = StageEnum.GetMapId(BootstrapPatch.mapInternalName).ToString();
        collection.assetBundleNames = new string[] {"foo", "bar"};
        collections.Add(collection);
        __instance.assetBundleLibrary.collections = collections.ToArray();
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Assets.GetAssetsToLoadDataForScene))]
    private static void GetAssetsToLoadDataForScene_Prefix(Assets __instance, string sceneName) {
        Debug.Log(sceneName);
    }
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Assets.GetAssetsToLoadDataForScene))]
    private static void GetAssetsToLoadDataForScene_Postfix(Assets __instance, string sceneName, ref AssetsToLoadData __result) {
        // TODO when trying to load a custom map, vanilla BRC doesn't know what to do here, gives us `null`
        // So we hook and create a reasonable value ourselves.  Is `false` for all correct?
        if(__result == null) {
            var data = ScriptableObject.CreateInstance<AssetsToLoadData>();
            data.loadCharacters = false;
            data.loadGameplayPrefabs = false;
            data.loadGraffiti = false;
            data.sfxCollectionsToLoad = new SfxCollection[0];
            __result = data;
        }
    }
}
