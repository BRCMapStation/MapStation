using System;
using System.Collections.Generic;
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

        private List<DebugMenu> debugMenus = [];

        private bool show = true;
        private DebugMenu currentDebugMenu = null;

        public void RegisterMenu(string name, Action onDebugUI) {
            var menu = new DebugMenu() { Name = name, OnGUI = onDebugUI };
            debugMenus.Add(menu);
        }

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
                    if (currentDebugMenu != null) {
                        if (GUILayout.Button("Back")) {
                            currentDebugMenu = null;
                        }
                        currentDebugMenu.OnGUI.Invoke();
                    } else {
                        foreach (var debugMenu in debugMenus) {
                            if (GUILayout.Button(debugMenu.Name))
                                currentDebugMenu = debugMenu;
                        }
                    }
                }
            } finally {
                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
        }

        private class DebugMenu {
            public string Name;
            public Action OnGUI;
        }
    }
}
