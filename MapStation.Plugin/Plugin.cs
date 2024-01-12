using BepInEx;
using System;
using Reptile;
using CommonAPI;
using BepInEx.Logging;
using BepInEx.Bootstrap;
using HarmonyLib;
using System.Linq;
using MapStation.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;

namespace MapStation.Plugin
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin {
        public static Plugin Instance;
        public static ManualLogSource Log = null;
        internal static bool DynamicCameraInstalled = false;
        internal static bool BunchOfEmotesInstalled = false;
        internal string TestMapsAbsoluteDirectory;

        // Hack: we must reference dependent assemblies from a class that's guaranteed to execute or else they don't
        // load and MonoBehaviours are missing.
        private static Type ForceLoadMapStationCommonAssembly = typeof(MapStation.Common.Dependencies.AssemblyDependencies);
        private static Type ForceLoadMapStationPluginAssembly = typeof(MapStation.Plugin.Dependencies.AssemblyDependencies);
        private static Type ForceLoadMapStationTypeForwarderAssembly = typeof(MapStation.TypeForwarder.Dependencies.AssemblyDependencies);

        private void Awake() {
            Instance = this;
            try {
                Initialize();
                Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} {PluginInfo.PLUGIN_VERSION} is loaded!");
            }
            catch(Exception e) {
                Logger.LogError($"Plugin {PluginInfo.PLUGIN_GUID} {PluginInfo.PLUGIN_VERSION} failed to load!{Environment.NewLine}{e}");
            }
        }

        private void Initialize() {
            // Our local dev workflow uses a nested folder, but at release we realized last-minute it was breaking R2.
            // So this code will detect the nested folder and use it if it exists.
            new MapStationConfig(Config);
            SceneNameMapper.Instance = new SceneNameMapper();
            ZipAssetBundles.Instance = new ZipAssetBundles();
            StageProgresses.Instance = new StageProgresses();
#if MAPSTATION_DEBUG
            DebugUI.Create(MapStationConfig.Instance.DebugUI.Value);
#endif
            TestMapsAbsoluteDirectory = Path.Combine(Path.GetDirectoryName(Plugin.Instance.Info.Location), PathConstants.TestMapsDirectory);

            Log = Logger;
            StageAPI.OnStagePreInitialization += StageAPI_OnStagePreInitialization;
            var harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll();

            BunchOfEmotesInstalled = Chainloader.PluginInfos.Keys.Contains("com.Dragsun.BunchOfEmotes");
            DynamicCameraInstalled = Chainloader.PluginInfos.Keys.Contains("DynamicCamera") || Chainloader.PluginInfos.Keys.Contains("com.Dragsun.Savestate");

            // Save paths to BepInExProfile and BRC installation to registry, for Editor to reference
            PathDetection.SetBepInExProfileInRegistry(BepInEx.Paths.BepInExRootPath);
            PathDetection.SetBRCPathInRegistry(Path.GetDirectoryName(Application.dataPath));

            UpdateEvent += LogScenes;
        }

        public void InitializeMapDatabase() {
            // Don't run this until after `Assets` have initialized!

            MapDatabase.Instance = new MapDatabase();

            if(!Directory.Exists(TestMapsAbsoluteDirectory)) {
                Directory.CreateDirectory(TestMapsAbsoluteDirectory);
            }

            foreach(var file in Directory.GetFiles(TestMapsAbsoluteDirectory)) {
                if(file.EndsWith(PathConstants.MapFileExtension)) {
                    var mapName = Path.GetFileNameWithoutExtension(file);
                    Debug.Log($"Found map {mapName} at {file}");
                    var properties = new MapProperties();
                    using(var zip = new MapZip(file)) {
                        JsonUtility.FromJsonOverwrite(zip.GetPropertiesText(), properties);
                    }
                    var map = new PluginMapDatabaseEntry() {
                        Name = mapName,
                        internalName = mapName,
                        Properties = properties,
                        ScenePath = AssetNames.GetScenePathForMap(mapName),
                        zipPath = file,
                        stageId = StageEnum.AddMapName(mapName)
                    };
                    MapDatabase.Instance.Add(map);
                }
            }
        }

        private void StageAPI_OnStagePreInitialization(Stage newStage, Stage previousStage) {

        }

        private void Update() {
            UpdateEvent?.Invoke();
        }

        private static void LogScenes() {
            if(Input.GetKeyDown(KeyCode.F6)) {
                foreach(var scene in SceneManager.GetAllScenes()) {
                    Debug.Log($"Scene {scene.buildIndex} {scene.name} {scene.path}");
                }
                foreach(var bundle in AssetBundle.GetAllLoadedAssetBundles()) {
                    Debug.Log($"Bundle {bundle.name}");
                    foreach(var p in bundle.GetAllScenePaths()) {
                        Debug.Log($"Bundle {bundle.name} has scene {p}");
                    }
                }
                foreach(var o in Resources.FindObjectsOfTypeAll(typeof(AssetBundle))) {
                    var bundle = o as AssetBundle;
                    Debug.Log($"Resources Bundle {bundle.name}");
                    foreach(var p in bundle.GetAllScenePaths()) {
                        Debug.Log($"Bundle {bundle.name} has scene {p}");
                    }
                }
                foreach(var p in Core.Instance.assets.availableBundles) {
                    Debug.Log($"{p.Key} {p.Value.currentState.ToString()} {p.Value.assetBundle}");
                    // if(p.Key == "maps/cspotcode.deatheggzone/scene") {
                    //     Debug.Log($"Unloading");
                    //     p.Value.assetBundle.Unload(unloadAllLoadedObjects: true);
                    // }
                    // if(p.Key == "maps/cspotcode.deatheggzone/assets") {
                    //     Debug.Log($"Unloading");
                    //     p.Value.Unload();
                    // }
                }
            }
            if(Input.GetKeyDown(KeyCode.F8)) {
                GameObject.FindFirstObjectByType<Bootstrap>().StartCoroutine(BackToHideout());
            }
        }

        private static IEnumerator BackToHideout() {
            yield return null;
            Core.Instance.BaseModule.SwitchStage(Stage.hideout);
        }

        public delegate void UpdateDelegate();
        public static UpdateDelegate UpdateEvent;
    }
}
