using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using Winterland.Common;

[CustomEditor(typeof(SetActionToRunOnEndAction))]
public class SetActionToRunOnEndActionEditor : Editor {
    public override void OnInspectorGUI() {
        var action = serializedObject.targetObject as SetActionToRunOnEndAction;
        var sequence = action.GetComponent<Sequence>();
        DrawDefaultInspector();
        if (!string.IsNullOrEmpty(action.ActionToRunOnEnd)) {
            if (sequence.GetActionByName(action.ActionToRunOnEnd) == null)
                EditorGUILayout.HelpBox($"Action {action.ActionToRunOnEnd} doesn't exist!", MessageType.Error);
        }
    }
}
