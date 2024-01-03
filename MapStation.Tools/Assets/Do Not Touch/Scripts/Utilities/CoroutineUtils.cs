#if UNITY_EDITOR
using System;
using System.Collections;
using Unity.EditorCoroutines.Editor;

static class CoroutineUtils {
    public delegate void RunNextTickDelegate();
    public static void RunNextTick(RunNextTickDelegate fn, Object owner = null) {
        if(owner == null)
            EditorCoroutineUtility.StartCoroutineOwnerless(RunNextTickWorker(fn));
        else
            EditorCoroutineUtility.StartCoroutine(RunNextTickWorker(fn), owner);
    }
    private static IEnumerator RunNextTickWorker(RunNextTickDelegate fn) {
        yield return null;
        fn();
    }
}
#endif