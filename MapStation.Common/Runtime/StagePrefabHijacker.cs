using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MapStation.Common {
    public static class StagePrefabHijacker {
#if BEPINEX
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
        private const int PublicToiletPool = 50;

        public static void Log(string text) {
            Common.Log.Info($"[StagePrefabHijacker] {text}");
        }

        private static void CleanUp() {
            if (Prefabs == null) return;
            if (Prefabs.Parent == null) return;
            GameObject.Destroy(Prefabs.Parent);
        }

        public static void Run() {
            CleanUp();

            Prefabs = new StagePrefabs();

            Prefabs.Parent = new GameObject("Custom Stage Prefabs");
            GameObject.DontDestroyOnLoad(Prefabs.Parent);
            Prefabs.Parent.SetActive(false);

            var toilet = GameObject.FindObjectOfType<PublicToilet>();
            for(var i = 0; i < PublicToiletPool; i++) {
                var newToilet = GameObject.Instantiate(toilet.gameObject);
                newToilet.transform.SetParent(Prefabs.Parent.transform, false);
                newToilet.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                Prefabs.Toilets.Add(newToilet);
            }
        }

        public class StagePrefabs {
            public GameObject Parent;
            public List<GameObject> Toilets = new();

            public GameObject GetToilet() {
                var toilet = Toilets[0];
                Toilets.RemoveAt(0);
                return toilet;
            }
        }
#endif
    }
}
