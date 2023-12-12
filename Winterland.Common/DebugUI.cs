using System;
using System.Net.Sockets;
using Reptile;
using UnityEngine;
using Winterland.Common;

namespace Winterland.Common {
    public class DebugUI : MonoBehaviour {
        private const int Width = 400;
        private const int Height = 1200;

        public static DebugUI Instance {get; private set;}

        public static void Create(bool enabled) {
            if(Instance != null) return;
            var gameObject = new GameObject("Winterland Debug UI");
            gameObject.SetActive(enabled);
            Instance = gameObject.AddComponent<DebugUI>();
            DontDestroyOnLoad(gameObject);
        }

        public Action OnDebugUI;

        private bool show = true;

        private void OnGUI () {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            GUILayout.BeginArea(new Rect(0, 0, Width, Height));
            GUILayout.BeginVertical("Debug UI", GUI.skin.box);
            GUILayout.Space(20);
            try {
                if(GUILayout.Button("Show/Hide")) {
                    show = !show;
                }
                if(show) {
                    OnDebugUI?.Invoke();
                }
            } finally {
                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
        }
    }
}
