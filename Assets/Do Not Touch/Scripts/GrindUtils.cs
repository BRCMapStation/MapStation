#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class GrindUtils {
    public static void autoSelectIfEnabled(GameObject o) {
        autoSelectIfEnabled(new GameObject[] {o});
    }
    public static void autoSelectIfEnabled(GameObject[] objects) {
        if (Preferences.instance.grinds.autoSelectNewNodes) {
            Selection.objects = objects;
        }
    }
}
#endif