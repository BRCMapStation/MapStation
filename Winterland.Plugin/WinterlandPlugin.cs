using BepInEx;
using System;
using Reptile;
using CommonAPI;

namespace Winterland.Plugin
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class WinterlandPlugin : BaseUnityPlugin
    {
        private void Awake()
        {
            try
            {
                StageAPI.OnStagePreInitialization += StageAPI_OnStagePreInitialization;
                Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} {PluginInfo.PLUGIN_VERSION} is loaded!");
            }
            catch(Exception e)
            {
                Logger.LogError($"Plugin {PluginInfo.PLUGIN_GUID} {PluginInfo.PLUGIN_VERSION} failed to load!{Environment.NewLine}{e}");
            }
        }

        private void StageAPI_OnStagePreInitialization(Stage newStage, Stage previousStage)
        {
            if (newStage != Stage.square)
                return;
            WinterManager.Create();
        }
    }
}
