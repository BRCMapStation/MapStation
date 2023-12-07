using System;
using System.Net.Sockets;
using Reptile;
using UnityEngine;
using Winterland.Common;

namespace Winterland.Common {
    public class DebugUI : MonoBehaviour {
        private const int Width = 400;
        private const int Height = 800;

        public static DebugUI Instance {get; private set;}

        public static void Create(bool enabled) {
            if(Instance != null) return;
            var gameObject = new GameObject("Winterland Debug UI");
            gameObject.SetActive(enabled);
            Instance = gameObject.AddComponent<DebugUI>();
            DontDestroyOnLoad(gameObject);
        }

        public Action OnDebugUI;

        private void OnGUI () {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            GUILayout.BeginArea(new Rect(0, 0, Width, Height));
            GUILayout.BeginVertical("Debug UI", GUI.skin.box);
            try {
                if(GUILayout.Button("Log message")) {
                    Debug.Log("Message from DebugUI");
                }
                OnDebugUI?.Invoke();
            } finally {
                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
        }
    }
}