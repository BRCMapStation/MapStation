using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MapStation.Plugin {
    public static class StagePrefabHijacker {
        public const Stage StageToHijackPrefabsFrom = Stage.hideout;
        public static string[] ProtectedAssetBundles = {
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
            "city_assets",
        };
        public static Stage ActualTargetStage = Stage.NONE;
        public static bool Loaded = false;
        public static bool Active = false;
        public static StagePrefabs Prefabs;

        public static void Log(string text) {
            Common.Log.Info($"[StagePrefabHijacker]{text}");
        }

        public static void Run() {
            Prefabs = new StagePrefabs();

            var prefabsParent = new GameObject("Custom Stage Prefabs");
            GameObject.DontDestroyOnLoad(prefabsParent);

            var toilet = GameObject.FindObjectOfType<PublicToilet>();
            var toiletInstance = GameObject.Instantiate(toilet);
            toiletInstance.transform.SetParent(prefabsParent.transform, false);
            toiletInstance.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            
            Prefabs.Toilet = toiletInstance;
        }

        public class StagePrefabs {
            public PublicToilet Toilet;
        }
    }
}
