using BepInEx;
using System;
using Reptile;

namespace Winterland.Plugin
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class WinterlandPlugin : BaseUnityPlugin
    {
        private void Awake()
        {
            try
            {
                StageManager.OnStagePostInitialization += StageManager_OnStagePostInitialization;
                Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} {PluginInfo.PLUGIN_VERSION} is loaded!");
            }
            catch(Exception e)
            {
                Logger.LogError($"Plugin {PluginInfo.PLUGIN_GUID} {PluginInfo.PLUGIN_VERSION} failed to load!{Environment.NewLine}{e}");
            }
        }

        private void StageManager_OnStagePostInitialization()
        {
            if (Core.Instance.BaseModule.CurrentStage != Stage.square)
                return;
            WinterManager.Create();
        }
    }
}
