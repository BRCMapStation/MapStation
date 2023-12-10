using BepInEx;
using System;
using Reptile;
using CommonAPI;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using System.Reflection;
using System.Linq;
using Winterland.Common;
using System.Runtime.CompilerServices;

namespace Winterland.Plugin
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency(WinterCharacters.CrewBoomGUID, BepInDependency.DependencyFlags.SoftDependency)]
    public class Plugin : BaseUnityPlugin {
        public static Plugin Instance;
        public static ManualLogSource Log = null;

        // Hack: we must reference dependent assemblies from a class that's guaranteed to execute or else they don't
        // load and MonoBehaviours are missing.
        private static Type ForceLoadMapStationCommonAssembly = typeof(Winterland.MapStation.Common.Dependencies.AssemblyDependencies);
        private static Type ForceLoadMapStationPluginAssembly = typeof(Winterland.MapStation.Plugin.Dependencies.AssemblyDependencies);

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
            var assetBundlesFolder = Path.Combine(Path.GetDirectoryName(Info.Location), "AssetBundles");
            var winterAssets = new WinterAssets(assetBundlesFolder);
            new WinterConfig(Config);
            WinterCharacters.Initialize();
            ObjectiveDatabase.Initialize(winterAssets.WinterBundle);
            DebugUI.Create(WinterConfig.Instance.DebugUI.Value);
            NetManager.Create();
            NetManagerDebugUI.Create();
            new WinterProgress();

            Log = Logger;
            StageAPI.OnStagePreInitialization += StageAPI_OnStagePreInitialization;
            var harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll();
        }

        private void StageAPI_OnStagePreInitialization(Stage newStage, Stage previousStage) {
            var winterManager = WinterManager.Create();
            winterManager.SetupStage(newStage);
        }

        private void Update() {
            UpdateEvent?.Invoke();
        }

        public delegate void UpdateDelegate();
        public static UpdateDelegate UpdateEvent;
    }
}
