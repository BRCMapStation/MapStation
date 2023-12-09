using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditorInternal;
using Winterland.Common;
using UnityEditor;

public static class EditorHelper {
    public static void MoveUp(SerializedObject serializedObject) {
        var targetObject = serializedObject.targetObject as OrderedComponent;
        if (targetObject == null)
            return;
        var other = targetObject.MoveUp();
        if (other != null)
            EditorUtility.SetDirty(other);
        EditorUtility.SetDirty(targetObject);
    }

    public static void MoveDown(SerializedObject serializedObject) {
        var targetObject = serializedObject.targetObject as OrderedComponent;
        if (targetObject == null)
            return;
        var other = targetObject.MoveDown();
        if (other != null)
            EditorUtility.SetDirty(other);
        EditorUtility.SetDirty(targetObject);
    }
}
