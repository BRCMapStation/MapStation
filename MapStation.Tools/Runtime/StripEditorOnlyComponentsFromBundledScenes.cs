
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif

/// <summary>
/// Put this attribute on any MonoBehaviour you want to be stripped from AssetBundles
/// This prevents BepInEx from logging errors about missing components
/// </summary>
public class StripFromAssetBundleSceneAttribute : Attribute {}

#if UNITY_EDITOR

public class StripComponentsFromAssetBundleScene : IProcessSceneWithReport {

    private static Type[] types;

    [InitializeOnLoadMethod]
    private static void OnLoad() {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        var types = TypeCache.GetTypesWithAttribute<StripFromAssetBundleSceneAttribute>().ToList();

        // HACK reorder types based on [RequireComponent()] dependencies.
        // Unity forbids removing a component if a peer requires it, so we have
        // to delete dependencies *last*.
        // Instead of writing some algorithm to figure this out, I'm hardcoding it here.
        if(types.Contains(typeof(BezierSpline))) {
            types.Remove(typeof(BezierSpline));
            types.Add(typeof(BezierSpline));
        }

        StripComponentsFromAssetBundleScene.types = types.ToArray();

        sw.Stop();
        // If this gets slow, it will affect every domain reload.
        // Debug.Log(sw.Elapsed);
    }

    public int callbackOrder { get { return 0; } }
    public void OnProcessScene(Scene scene, BuildReport report)
    {
        // Mutate the scene, I assume(?) these changes will be saved into the asset bundle
        foreach(var root in scene.GetRootGameObjects()) {
            foreach(var type in types) {
                foreach(var component in root.GetComponentsInChildren(type)) {
                    UnityEngine.Object.DestroyImmediate(component);
                }
            }
        }
    }
}

#endif