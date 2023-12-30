using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public class FireworkDebugUI : MonoBehaviour {
        public static FireworkDebugUI Instance { get; private set; }
        public static void Create() {
            if (Instance != null) return;
            var gameObject = new GameObject($"Winterland {nameof(FireworkDebugUI)}");
            Instance = gameObject.AddComponent<FireworkDebugUI>();
            DontDestroyOnLoad(gameObject);
        }

        private void Awake() {
            DebugUI.Instance.RegisterMenu("Fireworks", OnDebugUI);
        }

        private void OnDebugUI() {
            if (GUILayout.Button("Launch Fireworks")) {
                var fireworks = FindObjectOfType<FireworkHolder>(true);
                if (fireworks == null) return;
                fireworks.Launch();
            }

        }
    }
}
