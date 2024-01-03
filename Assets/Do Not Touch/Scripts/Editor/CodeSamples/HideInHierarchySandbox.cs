#if UNITY_EDITOR
using System.Collections;
using System.Linq;
using Reptile;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Messy, experimental code
/// </summary>
[ExecuteInEditMode]
class HideInHierarchy : MonoBehaviour {

    void OnEnable() {
        Selection.selectionChanged -= onSelectionChanged;
        Selection.selectionChanged += onSelectionChanged;
        Debug.Log("Install callback");
    }

    void OnDisable() {
        Selection.selectionChanged -= onSelectionChanged;
        Debug.Log("Uninstall callback");
    }

    void onSelectionChanged() {
        EditorCoroutineUtility.StartCoroutineOwnerless(onSelectionChangedCoro());
    }
    IEnumerator onSelectionChangedCoro() {
        Debug.Log("onSelectionChanged");
        yield return null;
        var replacement = GetComponentInParent<GrindLine>().gameObject;
        if (Selection.Contains(replacement)) {
            SceneVisibilityManager.instance.DisablePicking(gameObject, false);
            yield break;
        } else {
            SceneVisibilityManager.instance.EnablePicking(gameObject, false);
        }
        if (Selection.Contains(gameObject)) {
            for(int i = 0; i < Selection.objects.Count(); i++) {
                Debug.Log("selection " + i + ": " + Selection.objects[i]);
                if(Selection.objects[i] == gameObject) {
                    Debug.Log(Selection.objects[i]);
                    Object[] s = (Object[])Selection.objects.Clone();
                    Selection.objects[i] = replacement;
                    s[i] = replacement;
                    Selection.objects = s;
                    // Selection.objects = s;
                    // Selection.objects = new Object[0];
                    // Selection.SetActiveObjectWithContext(GetComponentInParent<GrindLine>().gameObject, null);
                    break;
                }
            }
        }
    }
    public bool serialized = false;
    private void OnValidate() {
        gameObject.hideFlags = HideFlags.None;
        hideFlags = HideFlags.None;

        // gameObject.hideFlags = HideFlags.HideInHierarchy;
        // hideFlags = HideFlags.DontSaveInEditor;

        // Debug.Log(gameObject);
        // Debug.Log(gameObject.hideFlags); 
        // Debug.Log(hideFlags);
    }
}
#endif