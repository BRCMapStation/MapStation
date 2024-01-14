
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

// TODO delete this file?  Or update it to auto-visualize properties for the currently-opened Map

public class BuildWindow : EditorWindow {
    const string WindowLabel = "Map Builder";

    [MenuItem(UIConstants.menuLabel + "/" + WindowLabel, priority = UIConstants.defaultMenuPriority)]
    private static void ShowMyEditor() {
        EditorWindow wnd = GetWindow<BuildWindow>();
        wnd.titleContent = new GUIContent(WindowLabel);
    }

    private Vector2 scrollbarPosition = Vector2.zero;

    private BRCMap brcMap_;
    private BRCMap brcMap => brcMap_ = brcMap_ != null ? brcMap_ : FindObjectOfType<BRCMap>();

    private Editor brcMapEditor = null;

    private void OnDestroy() {
        DestroyImmediate(brcMapEditor);
    }

    private void OnEnable() {
        EditorSceneManager.activeSceneChangedInEditMode -= onSceneChange;
        EditorSceneManager.activeSceneChangedInEditMode += onSceneChange;
    }
    private void OnDisable() {
        EditorSceneManager.activeSceneChangedInEditMode -= onSceneChange;
    }
    private void onSceneChange(Scene s, Scene s2) {
        brcMap_ = null;
    }

    private void OnInspectorUpdate() {
        Repaint();
    }

    private void OnGUI() {

        scrollbarPosition = EditorGUILayout.BeginScrollView(scrollbarPosition, false, false);

        if(brcMap) {
            Editor.CreateCachedEditor(brcMap, null, ref brcMapEditor);
            brcMapEditor.OnInspectorGUI();
        }

        EditorGUILayout.EndScrollView();
    }
}