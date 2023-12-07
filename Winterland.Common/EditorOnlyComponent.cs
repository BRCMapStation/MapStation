#if UNITY_EDITOR

// This is a test that our Common DLLs can include editor-only code

using UnityEngine;
using UnityEditor;

class EditorOnlyComponentFromWinterlandCommon : MonoBehaviour {
}

[CustomEditor(typeof(EditorOnlyComponentFromWinterlandCommon))]
class EditorOnlyComponentFromWinterlandCommonEditor : Editor {
    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("This help box is rendered using editor-only APIs from Winterland.Common", MessageType.None);
    }
}
#endif
