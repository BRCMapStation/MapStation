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
            Doctor.AnalyzeAndLog(stageAdditions);
        }

        private void Awake() {
            Instance = this;
            LightmapSettings.lightmaps = new LightmapData[] { };
            QualitySettings.shadowDistance = 150f;
        }
    }
}
