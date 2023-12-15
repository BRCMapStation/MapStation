using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Winterland.Common {
    public class LocalProgressDebugUI : MonoBehaviour {
        public static LocalProgressDebugUI Instance { get; private set; }
        public static void Create() {
            if (Instance != null) return;
            var gameObject = new GameObject($"Winterland {nameof(LocalProgressDebugUI)}");
            Instance = gameObject.AddComponent<LocalProgressDebugUI>();
            DontDestroyOnLoad(gameObject);
        }
        void Awake() {
            DebugUI.Instance.RegisterMenu("Local Progress", OnDebugUI);
        }
        private void OnDebugUI() {
            var localProgress = WinterProgress.Instance.LocalProgress;
            var toyLineManager = ToyLineManager.Instance;
            if (localProgress != null) {
                GUILayout.Label("Local Progress");
                GUILayout.Label($"Using {localProgress.GetType().Name}");
                GUILayout.Label($"[ILocalProgress] Gifts wrapped: {localProgress.Gifts}");
                GUILayout.Label($"[ILocalProgress] Faux head juggle high score: {localProgress.FauxJuggleHighScore}");
                GUILayout.Label($"[ILocalProgress] Current objective: {localProgress.Objective.name}");
                if (GUILayout.Button("Reset Progress")) {
                    localProgress.InitializeNew();
                    localProgress.Save();
                }
            }
            if (toyLineManager != null) {
                GUILayout.Label("Toy Line Manager");
                GUILayout.Label($"[ToyLineManager] Collected all Toy Lines: {toyLineManager.GetCollectedAllToyLines()}");
                GUILayout.Label($"[ToyLineManager] Toy Lines in this stage: {toyLineManager.ToyLines.Count}");
                if (GUILayout.Button("Respawn Toy Lines")) {
                    toyLineManager.RespawnAllToyLines();
                }
                if (GUILayout.Button("Collect all Toy Lines")) {
                    foreach(var toyLine in toyLineManager.ToyLines) {
                        toyLine.Collect();
                    }
                }
            }
            
        }
    }
}
