using System.Collections.Generic;
using System.Linq;
using MapStation.Common;
using Reptile;
using UnityEngine;
using System.IO;
using BepInEx.Logging;

namespace MapStation.Plugin;

public enum MapSource {
    Normal,
    TestMaps
}

public class MapDatabase {
    public static MapDatabase Instance;
    public Assets Assets;

    // I think it's better to use the Stage ID as the key here
    public Dictionary<Stage, PluginMapDatabaseEntry> maps = new ();

    private ManualLogSource logger;

    public MapDatabase(Assets assets) {
        Assets = assets;
        logger = new ManualLogSource("MapStation Map Database");
    }

    public void AddFromDirectory(string path, MapSource source = MapSource.Normal) {
        var files = Directory.GetFiles(path, $"*{PathConstants.MapFileExtension}", SearchOption.AllDirectories);
        foreach(var file in files) {
            var mapName = Path.GetFileNameWithoutExtension(file);
            var stageID = StageEnum.HashMapName(mapName);
            if (maps.ContainsKey(stageID)) {
                logger.LogWarning($"Won't add custom stage {mapName} to the database as there's already a stage with the same ID ({(int) stageID})");
            } else {
                logger.LogInfo($"Found map {mapName} at {file}");
                var properties = new MapProperties();
                using (var zip = new MapZip(file)) {
                    JsonUtility.FromJsonOverwrite(zip.GetPropertiesText(), properties);
                    zip.DoPluginOnAddMapToDatabase(mapName);
                }
                var map = new PluginMapDatabaseEntry() {
                    Name = mapName,
                    internalName = mapName,
                    Properties = properties,
                    ScenePath = AssetNames.GetScenePathForMap(mapName),
                    zipPath = file,
                    stageId = stageID,
                    source = source
                };
                Add(map);
            }
        }
    }

    public void Add(PluginMapDatabaseEntry map) {
        maps.Add(map.stageId, map);
        StageEnum.AddMapName(map.stageId, map.internalName);

        var collections = Assets.assetBundleLibrary.collections.ToList();
        var collection = ScriptableObject.CreateInstance<AssetBundleCollection>();
        collection.assetBundleCollectionName = map.stageId.ToString();
        // This list was copied from hideout, hopefully we don't need all of these?

        // reduced this a bit - i expect map makers will mostly extract assets from a game decompilation if they need simple things like textures or models
        // leaving common assets and basic stuff here.
        collection.assetBundleNames = new string[] {
            "characters",
            "graffiti",
            "common_assets",
            "enemies",
            "enemy_animation",
            "character_animation",
            "in_game_assets",
            "mocap_animation_two",
            "mocap_animation",
            "finalboss_assets",
            "playeranimation",
            "finalboss_animation",
            "storyanimation",
            "minimap",
            "common_game_shaders",
            "city_assets", // Vending machines require this one
                
            map.AssetsBundleName,
            map.SceneBundleName
        };
        collections.Add(collection);
        Assets.assetBundleLibrary.collections = collections.ToArray();

        foreach (var bundle in new[] {map.AssetsBundleName, map.SceneBundleName}) {
            Assets.availableBundles.Add(bundle, new Bundle(bundle) {
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
    public MapSource source;
}
