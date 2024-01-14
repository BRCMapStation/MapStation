using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class AssemblyLocatorWindow : EditorWindow {
    const string windowLabel = "Assembly Locator";

    [MenuItem(UIConstants.menuLabel + "/" + UIConstants.experimentsSubmenuLabel + "/" + windowLabel, priority = (int)UIConstants.MenuOrder.EXPERIMENTS)]
    private static void ShowMyEditor() {
        EditorWindow wnd = GetWindow<AssemblyLocatorWindow>();
        wnd.titleContent = new GUIContent(windowLabel);
    }

    private Vector2 scrollbarPosition = Vector2.zero;

    private string TypeName;
    private void OnGUI() {
        TypeName = EditorGUILayout.TextField(TypeName);
        if(GUILayout.Button("Search")) {
            foreach(var asm in AppDomain.CurrentDomain.GetAssemblies()) {
                foreach(var type in asm.GetTypes()) {
                    if(type.Name == TypeName) {
                        Debug.LogFormat("Found {0} {1}", type.AssemblyQualifiedName, type.Assembly.Location);
                    }
                }
            }
        }
    }
}