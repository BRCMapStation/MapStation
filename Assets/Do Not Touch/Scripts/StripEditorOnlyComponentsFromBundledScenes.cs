
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


        // Discover every class annotated to be stripped when scene is bundled
        // IEnumerable<Type> acc = null;
        // Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        // foreach (var assembly in assemblies)
        // {
        //     var types = assembly.GetTypes().Where(
        //         t => t.IsDefined(typeof(StripFromAssetBundleSceneAttribute))
        //     );
        //     acc = acc == null ? types : acc.Concat(types);
        // }

        // types = acc.ToArray();

        types = TypeCache.GetTypesWithAttribute<StripFromAssetBundleSceneAttribute>().ToArray();

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