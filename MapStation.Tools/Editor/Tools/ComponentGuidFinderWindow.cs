using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class ComponentGuidFinderWindow : EditorWindow {
    const string windowLabel = "Component Guid Finder";

    [MenuItem(UIConstants.menuLabel + "/" + UIConstants.experimentsSubmenuLabel + "/" + windowLabel, priority = (int)UIConstants.MenuOrder.EXPERIMENTS)]
    private static void ShowMyEditor() {
        EditorWindow wnd = GetWindow<ComponentGuidFinderWindow>();
        wnd.titleContent = new GUIContent(windowLabel);
    }

    private Vector2 scrollbarPosition = Vector2.zero;

    private string ComponentName;
    private string Output = "";
    private void OnGUI() {
        EditorGUILayout.HelpBox(
            "Enter the name of a MonoBehaviour and click Search.\n" +
            "Output will show yaml-formatted GUID and localId pairs for the component.\n\n" +
            "This may be helpful when fixing missing scripts or find-and-replacing DLL " +
            "components to point to their Package Manager counterparts.",
            MessageType.None);
        GUILayout.Space(10);

        ComponentName = EditorGUILayout.TextField(ComponentName);

        if(GUILayout.Button("Search")) {
            Output = "";
            foreach(var path in AssetDatabase.GetAllAssetPaths()) {
                if(!path.EndsWith(".cs") && !path.EndsWith(".dll")) continue;
                foreach(var asset in AssetDatabase.LoadAllAssetsAtPath(path)) {
                    if(asset is MonoScript script) {
                        if(asset.name == ComponentName) {
                            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(asset, out var Guid, out long localId);
                            Output += string.Format("m_Script: {{fileID: {0}, guid: {1}, type: 3}} # {2} /{3}\n", localId, Guid, script.name, path);
                        }
                    }
                }
            }
        }

        GUILayout.Space(10);
        GUILayout.Label("Results");
        EditorGUILayout.SelectableLabel(Output, EditorStyles.textArea);
    }
}