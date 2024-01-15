using MapStation.Tools;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Scene = UnityEngine.SceneManagement.Scene;
using static cspotcode.UnityGUI.GUIUtil;
using static UnityEditor.EditorGUILayout;
using static UnityEngine.GUILayout;

public class MapPropertiesWindow : EditorWindow {
    const string WindowLabel = "Map Properties";

    [MenuItem(UIConstants.menuLabel + "/" + WindowLabel, priority = (int)UIConstants.MenuOrder.MAP_PROPERTIES)]
    private static void ShowMyEditor() {
        EditorWindow wnd = GetWindow<MapPropertiesWindow>();
        wnd.titleContent = new GUIContent(WindowLabel);
    }

    private Vector2 scrollbarPosition = Vector2.zero;

    private EditorMapDatabaseEntry map = null;
    private MapPropertiesScriptableObject mapProperties = null;

    private Editor propertiesEditor = null;

    private void OnDestroy() {
        DestroyImmediate(propertiesEditor);
    }

    private void OnEnable() {
        EditorSceneManager.activeSceneChangedInEditMode -= onSceneChange;
        EditorSceneManager.activeSceneChangedInEditMode += onSceneChange;
    }
    private void OnDisable() {
        EditorSceneManager.activeSceneChangedInEditMode -= onSceneChange;
    }
    private void onSceneChange(Scene current, Scene next) {
        map = null;
        mapProperties = null;
    }

    private void OnInspectorUpdate() {
        Repaint();
    }

    private void OnGUI() {
        using(ScrollView(ref scrollbarPosition)) {
            if(map == null) {
                map = MapDatabase.GetMapForActiveScene();
                if(map == null) {
                    Label("To see map properties, open a map scene.");
                }
                return;
            }
            if(mapProperties == null) {
                mapProperties = map.Properties;
                if(mapProperties == null) {
                    Label($"Map {map.Name} is missing Properties file.  Create it?");
                    if(Button($"Create {map.PropertiesPath}")) {
                        MapDatabase.CreatePropertiesForMap(map);
                    }
                }
                return;
            }

            Editor.CreateCachedEditor(mapProperties, null, ref propertiesEditor);
            propertiesEditor.OnInspectorGUI();
        }
    }
}