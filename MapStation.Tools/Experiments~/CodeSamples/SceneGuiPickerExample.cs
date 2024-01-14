using UnityEditor;
using UnityEngine;

class SceneGUIPickerExample : EditorWindow {
    private void OnEnable() {
        SceneView.duringSceneGui -= duringSceneGui;
        SceneView.duringSceneGui += duringSceneGui;
    }

    private void OnDisable() {
        SceneView.duringSceneGui -= duringSceneGui;
    }
    
    /// Listener for SceneView.duringSceneGui
    private void duringSceneGui(SceneView v) {
        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            RaycastHit hit; 
            if (Physics.Raycast(Camera.current.ScreenPointToRay(
                new Vector3(e.mousePosition.x, Camera.current.pixelHeight - e.mousePosition.y, 0)),
                out hit,
                Mathf.Infinity))
            {
                Debug.Log("clicked at " + hit.point);
                e.Use(); // Suppress normal event action

//     PrefabUti1ity. GetPrefabParent (obi ectToInstantiate) 
// GameObiect placedObi ect = 
// (GameObi ect) PrefabUti1ity. InstantiatePrefab ( 
// placedObiect. transform. position = hit. point; 
// placedObiect . transform. local Scale = 
// new Vector3 (1, 1, 1) Random. Range (C . Sf, 2. Of) ; 
            }
        }
    }
}