using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Winterland.Common;
using Winterland.MapStation.Common;

namespace Winterland.Plugin
{
    /// <summary>
    /// Global controller, instantiated on all stages.
    /// </summary>
    public class WinterManager : MonoBehaviour {
        public static WinterManager Instance { get; private set; }

        public static WinterManager Create() {
            var gameObject = new GameObject("Winter Manager");
            return gameObject.AddComponent<WinterManager>();
        }

        public void SetupStage(Stage stage) {
            var stagePrefab = WinterAssets.Instance.GetPrefabForStage(stage);
            if (stagePrefab == null)
                return;
            var stageAdditions = GameObject.Instantiate(stagePrefab);
            var stageObjects = stageAdditions.GetComponentsInChildren<StageObject>(true);
            foreach(var stageObject in stageObjects) {
                stageObject.PutInChunk();
            }
            Doctor.AnalyzeAndLog(stageAdditions);
        }

        private void SetupUI() {
            var winterBundle = WinterAssets.Instance.WinterBundle;
            if (winterBundle == null)
                return;
            var uiPrefab = winterBundle.LoadAsset<GameObject>("WinterUI");
            if (uiPrefab == null)
                return;
            var winterUIInstance = GameObject.Instantiate(uiPrefab);
            var uiManager = Core.Instance.UIManager;
            // putting it this deep down into gameplay ui makes it disappear during graffiti minigame and stuff which we want!
            var gameplayUI = uiManager.transform.Find("GamePlayUI(Clone)").Find("GameplayUI").Find("gameplayUIScreen");
            winterUIInstance.transform.SetParent(gameplayUI, false);
        }

        private void Awake() {
            Instance = this;
            LightmapSettings.lightmaps = new LightmapData[] { };
            QualitySettings.shadowDistance = 150f;
            StageManager.OnStagePostInitialization += StageManager_OnStagePostInitialization;
        }

        private void OnDestroy() {
            Instance = null;
            StageManager.OnStagePostInitialization -= StageManager_OnStagePostInitialization;
        }

        private void StageManager_OnStagePostInitialization() {
            SetupUI();
        }
    }
}
