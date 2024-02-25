using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Reptile;
using UnityEngine;

namespace MapStation.Common {
    public class DebugUI : MonoBehaviour {
        public const int DefaultWidth = 400;
        private const int Height = 1200;

        public static DebugUI Instance {get; private set;}

        public static void Create(bool enabled) {
            if(Instance != null) return;
            var gameObject = new GameObject("MapStation Debug UI");
            gameObject.SetActive(enabled);
            Instance = gameObject.AddComponent<DebugUI>();
            DontDestroyOnLoad(gameObject);
        }

        public bool UiEnabled {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        private List<DebugMenu> debugMenus = new ();

        private bool show = true;
        private DebugMenu currentDebugMenu = null;

        public void RegisterMenu(DebugMenu menu) {
            debugMenus.Add(menu);
        }

        private void OnGUI () {
            var width = show && currentDebugMenu != null && currentDebugMenu.Width.HasValue ? currentDebugMenu.Width.Value : DefaultWidth;
            GUILayout.BeginArea(new Rect(0, 0, width, Height));
            // HACK call it multiple times to increase the opacity (yes the internet recommended this)
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.BeginVertical(GUI.skin.box);
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
                        } else {
                            currentDebugMenu.OnGUI();
                        }
                    } else {
                        foreach (var debugMenu in debugMenus) {
                            if (GUILayout.Button(debugMenu.Name))
                                currentDebugMenu = debugMenu;
                        }
                    }
                }
            } finally {
                GUILayout.EndVertical();
                GUILayout.EndVertical();
                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
        }

        public abstract class DebugMenu {
            public abstract string Name { get; }
            public virtual int? Width { get; } = null;
            public abstract void OnGUI();
        }
    }
}
