#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// When an object is destroyed, then re-created by undo,
/// C# references to the object before deletion still point to a C# object that
/// believes its destroyed.
/// 
/// This utility helps replacing these defunct C# objects with a new one which
/// believes it's alive.
/// </summary>
public static class ReferenceRecoveryUtil {
    public static void Repair<T>(ref T obj) where T : Object {
        if(ReferenceEquals(obj, null)) return;
        var currentObj = EditorUtility.InstanceIDToObject(obj.GetInstanceID());
        // If reference is still missing, leave it missing; don't replace with `null`
        if(ReferenceEquals(currentObj, null)) return;
        if(!ReferenceEquals(obj, currentObj)) {
            obj = (T)currentObj;
        }
    }

    public static T GetCurrent<T>(T obj) where T : Object {
        if(ReferenceEquals(obj, null)) return null;
        return (T)EditorUtility.InstanceIDToObject(obj.GetInstanceID());
    }

    public static bool GetCurrent<T>(T obj, out T currentObj) where T : Object {
        if(ReferenceEquals(obj, null)) {
            currentObj = null;
            return false;
        }
        if(obj != null) {
            currentObj = obj;
            return false;
        }
        currentObj = (T)EditorUtility.InstanceIDToObject(obj.GetInstanceID());
        return !ReferenceEquals(currentObj, null) && !ReferenceEquals(obj, currentObj);
    }
}
#endif