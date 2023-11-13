using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
            var stagePrefab = Plugin.Assets.GetPrefabForStage(stage);
            if (stagePrefab == null)
                return;
            GameObject.Instantiate(stagePrefab);
        }

        private void Awake() {
            Instance = this;
        }
    }
}
