using BepInEx;
using System;
using Reptile;
using CommonAPI;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using System.Reflection;
using System.Linq;

namespace Winterland.Plugin
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin {
        public static ManualLogSource Log = null;
        public static WinterConfig WinterConfig = null;
        public static WinterAssets Assets = null;

        private void Awake() {
            try {
                Initialize();
                Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} {PluginInfo.PLUGIN_VERSION} is loaded!");
            }
            catch(Exception e) {
                Logger.LogError($"Plugin {PluginInfo.PLUGIN_GUID} {PluginInfo.PLUGIN_VERSION} failed to load!{Environment.NewLine}{e}");
            }
        }

        private void Initialize() {
            Logger.LogInfo($"Forcing Mono assembly to load. {typeof(Common.ToyLine)}");
            Log = Logger;
            WinterConfig = new WinterConfig(Config);
            var assetBundlesFolder = Path.Combine(Path.GetDirectoryName(Info.Location), "AssetBundles");
            Assets = new WinterAssets(assetBundlesFolder);
            StageAPI.OnStagePreInitialization += StageAPI_OnStagePreInitialization;
            var harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll();
        }

        private void StageAPI_OnStagePreInitialization(Stage newStage, Stage previousStage) {
            var winterManager = WinterManager.Create();
            winterManager.SetupStage(newStage);
        }
    }
}
