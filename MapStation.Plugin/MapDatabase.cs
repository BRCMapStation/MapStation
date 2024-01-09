using System.Collections.Generic;
using System.Linq;
using MapStation.Common;
using Reptile;
using UnityEngine;

namespace MapStation.Plugin;

public class MapDatabase {
    public static MapDatabase Instance;

    public Dictionary<string, PluginMapDatabaseEntry> maps = new ();

    public void Add(PluginMapDatabaseEntry map) {
        maps.Add(map.internalName, map);
        StageEnum.AddMapName(map.stageId, map.internalName);

        var __instance = GameObject.FindObjectOfType<Bootstrap>().assets;

        var collections = __instance.assetBundleLibrary.collections.ToList();
        var collection = ScriptableObject.CreateInstance<AssetBundleCollection>();
        collection.assetBundleCollectionName = map.stageId.ToString();
        // This list was copied from hideout, hopefully we don't need all of these?
        collection.assetBundleNames = new string[] {
            "hideout",
            "characters",
            "graffiti",
            "hideout_sequences",
            "square_assets",
            "pyramid_assets",
            "common_assets",
            "osaka_assets",
            "enemies",
            "enemy_animation",
            "character_animation",
            "city_assets",
            "mall_assets",
            "hideout_pyramid_skybox",
            "hideout_assets",
            "hideout_buildable_assets",
            "in_game_assets",
            "mocap_animation_two",
            "mocap_animation",
            "osaka_skybox",
            "tower_assets",
            "finalboss_assets",
            "prelude_assets",
            "downhill_assets",
            "pyramid_skybox",
            "playeranimation",
            "finalboss_animation",
            "storyanimation",
            "minimap",
            "common_game_shaders",
            "hideout_combined_meshes",
                
            map.AssetsBundleName,
            map.SceneBundleName
        };
        collections.Add(collection);
        __instance.assetBundleLibrary.collections = collections.ToArray();

        foreach (var bundle in new[] {map.AssetsBundleName, map.SceneBundleName}) {
            __instance.availableBundles.Add(bundle, new Bundle(bundle) {
                name = bundle
            });
        }

        ZipAssetBundles.Instance.Bundles.Add(map.AssetsBundleName, new ZipAssetBundle {
            zipPath = map.zipPath,
            bundleType = ZipBundleType.ASSETS,
        });
        ZipAssetBundles.Instance.Bundles.Add(map.SceneBundleName, new ZipAssetBundle {
            zipPath = map.zipPath,
            bundleType = ZipBundleType.SCENE,
        });

        SceneNameMapper.Instance.Names.Add(map.stageId.ToString(), $"{AssetNames.SceneBasenamePrefix}{map.internalName}");
        SceneNameMapper.Instance.Paths.Add(map.stageId.ToString(), $"Assets/{AssetNames.MapDirectory}/{map.internalName}/{AssetNames.SceneBasenamePrefix}{map.internalName}.unity");
    }
}

public class PluginMapDatabaseEntry : BaseMapDatabaseEntry {
    public Stage stageId;
    public string internalName;
    public string zipPath;
    public MapProperties Properties;
}
