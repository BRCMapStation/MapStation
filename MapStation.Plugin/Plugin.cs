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
using MapStation.Plugin.Phone;
using CommonAPI.Phone;
using MapStation.API;
using MapStation.Plugin.API;
using MapStation.Plugin.Tools;

namespace MapStation.Plugin
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin {
        public static Plugin Instance;
        public static ManualLogSource Log = null;
        // Directory for maps bundled with MapStation, such as the subway station. These take top priority, can't be overriden.
        internal string MapStationMapsAbsoluteDirectory;
        // Test map directory, lower priority than map station maps.
        internal string TestMapsAbsoluteDirectory;
        // User maps in plugins folder - lowest priority.
        internal string UserMapsAbsoluteDirectory;

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

            DebugUI.Create(MapStationConfig.Instance.DebugUI.Value);
            DebugUI.Instance.RegisterMenu(new BackToHideoutDebugUI());
            DebugUI.Instance.RegisterMenu(new DoctorDebugUI());

            MapStationMapsAbsoluteDirectory = Path.GetDirectoryName(Info.Location);
            TestMapsAbsoluteDirectory = Path.Combine(Paths.ConfigPath, PathConstants.ConfigDirectory, PathConstants.TestMapsDirectory);
            UserMapsAbsoluteDirectory = Paths.PluginPath;

            ThreadedLogFix.Install();

            Log = Logger;
            PhoneAPI.RegisterApp<AppMapStation>("mapstation");
            var harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll();

            // Save paths to BepInExProfile and BRC installation to registry, for Editor to reference
            PathDetection.SetBepInExProfileInRegistry(BepInEx.Paths.BepInExRootPath);
            PathDetection.SetBRCPathInRegistry(Path.GetDirectoryName(Application.dataPath));

            StageAPI.OnStagePreInitialization += MapRepair.OnStagePreInitialization;
            StageAPI.OnStagePreInitialization += EnableDebugFeaturesOnStageInit;
        }

        public void InitializeMapDatabase(Assets assets) {
            // Don't run this until after `Assets` have initialized!

            MapDatabase.Instance = new MapDatabase(assets);

            if(!Directory.Exists(TestMapsAbsoluteDirectory)) {
                Directory.CreateDirectory(TestMapsAbsoluteDirectory);
            }
            MapDatabase.Instance.AddFromDirectory(MapStationMapsAbsoluteDirectory);
            MapDatabase.Instance.AddFromDirectory(TestMapsAbsoluteDirectory, MapSource.TestMaps);
            MapDatabase.Instance.AddFromDirectory(UserMapsAbsoluteDirectory);

            var api = new MapStationAPI(MapDatabase.Instance);
            APIManager.Initialize(api);
        }

        private void Update() {
            UpdateEvent?.Invoke();
        }

        private static IEnumerator BackToHideout() {
            yield return null;
            Core.Instance.BaseModule.SwitchStage(Stage.hideout);
        }

        public delegate void UpdateDelegate();
        public static UpdateDelegate UpdateEvent;

        public static bool CanSwitchStagesWithoutCrashing() {
            return Core.Instance != null && Core.Instance.BaseModule.IsPlayingInStage && !Core.Instance.BaseModule.IsLoading && !Core.Instance.BaseModule.IsInGamePaused;
        }
        private static void QuickReloadUpdate() {
            // Only allow if the game is currently in a stage
            if(CanSwitchStagesWithoutCrashing()) {
                if(Input.GetKeyDown(KeyCode.F5)) {
                    Core.Instance.BaseModule.StageManager.ExitCurrentStage(Core.Instance.SaveManager.CurrentSaveSlot.currentStageProgress.stageID);
                }
            }
        }

        private void EnableDebugFeaturesOnStageInit(Stage newStage, Stage previousStage) {
            var isLocalBuild = MapDatabase.Instance.maps.TryGetValue(newStage, out var map) && map.source == MapSource.TestMaps;
            if(MapStationConfig.Instance.DebugUI.Value) {
                DebugUI.Instance.enabled = true;
            } else {
                DebugUI.Instance.UiEnabled = isLocalBuild;
            }
            if(MapStationConfig.Instance.QuickReloadValue) {
                UpdateEvent += QuickReloadUpdate;
            }

            UpdateEvent -= QuickReloadUpdate;
            if(MapStationConfig.Instance.QuickReloadValue || isLocalBuild) {
                UpdateEvent += QuickReloadUpdate;
            }
        }
    }
}
