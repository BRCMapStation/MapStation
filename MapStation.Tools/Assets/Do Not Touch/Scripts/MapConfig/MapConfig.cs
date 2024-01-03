using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ClassInMapConfigAssembly : MonoBehaviour
{
    #if UNITY_EDITOR
    [InitializeOnLoadMethod]
    static void OnLoad() {
        // Debug.Log(nameof(ClassInMapConfigAssembly) + " exists in assembly: " + typeof(ClassInMapConfigAssembly).Assembly);
    }
    #endif
}
