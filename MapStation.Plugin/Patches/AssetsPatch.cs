using System.Collections;
using System.IO;
using HarmonyLib;
using Reptile;
using UnityEngine;
using MapStation.Common;

namespace MapStation.Plugin.Patches;

[HarmonyPatch(typeof(Assets))]
internal static class AssetsPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Assets.Init))]
    private static void Init_Postfix(Assets __instance) {
        Plugin.Instance.InitializeMapDatabase(__instance);
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Assets.GetAssetsToLoadDataForScene))]
    private static void GetAssetsToLoadDataForScene_Postfix(Assets __instance, string sceneName, ref AssetsToLoadData __result) {
        // When trying to load a custom map, vanilla BRC doesn't know what to do here, gives us `null`,
        // so we create a value here.
        // Vanilla stages use identical values: true, true, true, empty array of SfxCollections
        if(__result == null) {
            Log.Info($"{nameof(Assets)}.{nameof(Assets.GetAssetsToLoadDataForScene)} does not know about scene {sceneName}; assuming it's a custom map, creating default struct instead.");
            __result = ScriptableObject.CreateInstance<AssetsToLoadData>();
            __result.loadCharacters = true;
            __result.loadGameplayPrefabs = true;
            __result.loadGraffiti = true;
            __result.sfxCollectionsToLoad = new SfxCollection[0];
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Assets.LoadBundleASync))]
    private static bool LoadBundleASync_Prefix(ref IEnumerator __result, Assets __instance, Bundle bundleToLoad)
    {
        // Fully replace implementation of LoadBundleASync to load from map zips
        if(ZipAssetBundles.Instance.Bundles.TryGetValue(bundleToLoad.name, out var zipAssetBundle)) {
            __result = LoadMapBundleASync(__instance, bundleToLoad, zipAssetBundle);
            return false;
        }
        return true;
    }
    
    /// <summary>
    /// Alternative implementation of Assets.LoadBundleASync which loads from a custom map zip
    /// </summary>
    private static IEnumerator LoadMapBundleASync(Assets __instance, Bundle bundleToLoad, ZipAssetBundle zipAssetBundle)
    {
        bundleToLoad.InitializeLoad();
        
        Log.Info($"{nameof(Assets)}.{nameof(Assets.LoadBundleASync)} loading {bundleToLoad.Name} from zip {zipAssetBundle.zipPath}");
        byte[] data;
        using(var zip = new MapZip(zipAssetBundle.zipPath)) {
            Stream stream = zipAssetBundle.bundleType switch {
                ZipBundleType.SCENE => zip.GetSceneBundleStream(),
                ZipBundleType.ASSETS => stream = zip.GetAssetsBundleStream(),
                _ => throw new System.Exception($"Unexpected {nameof(ZipBundleType)} {zipAssetBundle.bundleType.ToString()}")
            };
            using (stream)
            using (var ms = new MemoryStream()) {
                stream.CopyTo(ms);
                data = ms.ToArray();
            }
        }
        __instance.currentAssetBundleCreateRequest = AssetBundle.LoadFromMemoryAsync(data);

        yield return __instance.currentAssetBundleCreateRequest;
        if (__instance.currentAssetBundleCreateRequest.assetBundle == null)
        {
            bundleToLoad.ResetLoadState();
        }
        else {
            bundleToLoad.SetAssetBundle(__instance.currentAssetBundleCreateRequest.assetBundle);
        }
    }
}
