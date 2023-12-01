#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class GrindUtils {
    public static void autoSelectIfEnabled(GameObject o) {
        autoSelectIfEnabled(new GameObject[] {o});
    }
    public static void autoSelectIfEnabled(List<GameObject> objects) {
        autoSelectIfEnabled(objects.ToArray());
    }
    public static void autoSelectIfEnabled(GameObject[] objects) {
        if (Preferences.instance.grinds.autoSelectNewNodes) {
            Selection.objects = objects;
        }
    }
}
#endif