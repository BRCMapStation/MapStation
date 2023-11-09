using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class RaceCheckpoint : MonoBehaviour {
    #if UNITY_EDITOR
    [InitializeOnLoadMethod]
    static void OnLoad() {
        // Debug.Log(nameof(RaceCheckpoint) + " exists in assembly: " + typeof(RaceCheckpoint).Assembly);
    }
    #endif
}