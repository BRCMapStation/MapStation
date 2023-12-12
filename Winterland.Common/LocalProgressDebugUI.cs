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
            DebugUI.Instance.OnDebugUI += OnDebugUI;
        }
        private void OnDebugUI() {
            var localProgress = WinterProgress.Instance.LocalProgress;
            var toyLineManager = ToyLineManager.Instance;
            if (localProgress != null) {
                GUILayout.Label("Local Progress");
                GUILayout.Label($"Using {localProgress.GetType().Name}");
                GUILayout.Label($"[ILocalProgress] Gifts wrapped: {localProgress.Gifts}");
                GUILayout.Label($"[ILocalProgress] Current objective: {localProgress.Objective.name}");
            }
            if (toyLineManager != null) {
                GUILayout.Label("Toy Line Manager");
                GUILayout.Label($"[ToyLineManager] Collected all Toy Lines: {ToyLineManager.Instance.GetCollectedAllToyLines()}");
                GUILayout.Label($"[ToyLineManager] Toy Lines in this stage: {ToyLineManager.Instance.ToyLines.Count}");
            }
            
        }
    }
}
