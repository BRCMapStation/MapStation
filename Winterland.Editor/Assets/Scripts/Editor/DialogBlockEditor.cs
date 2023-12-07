using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using Winterland.Common;

[CustomEditor(typeof(DialogBlock))]
public class DialogBlockEditor : Editor {
    public override void OnInspectorGUI() {
        var dialogBlock = serializedObject.targetObject as DialogBlock;
        if (dialogBlock.Speaker == DialogBlock.SpeakerMode.Text) {
            EditorGUI.BeginChangeCheck();
            var nameProperty = serializedObject.FindProperty("SpeakerName");
            var speakerName = EditorGUILayout.TextField(nameProperty.displayName, nameProperty.stringValue);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(target, "Changed Speaker Name");
                dialogBlock.SpeakerName = speakerName;
            }
        }
        DrawDefaultInspector();
    }
}
