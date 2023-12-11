using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using Winterland.Common;
using static PlasticGui.PlasticTableColumn;

[CustomEditor(typeof(ToyPart))]
public class ToyPartEditor : Editor {

    public static void Randomize(ToyPart toyPart) {
        var visual = toyPart.transform.Find("Part Visual");
        if (visual == null)
            return;
        var renderers = visual.GetComponentsInChildren<Renderer>(true);

        var rendererIndex = UnityEngine.Random.Range(0, renderers.Length);
        for (var i = 0; i < renderers.Length; i++) {
            var go = renderers[i].gameObject;
            EditorUtility.SetDirty(go);
            if (i == rendererIndex)
                go.SetActive(true);
            else
                go.SetActive(false);
        }
    }

    public override void OnInspectorGUI() {
        var toyPart = serializedObject.targetObject as ToyPart;
        var visual = toyPart.transform.Find("Part Visual");
        if (visual == null)
            return;
        var toyDictionary = new Dictionary<string, List<string>>();
        var renderers = visual.GetComponentsInChildren<Renderer>(true);

        DrawDefaultInspector();

        EditorGUILayout.LabelField("Visuals", EditorStyles.boldLabel);

        if (GUILayout.Button("Randomize")) {
            Randomize(toyPart);
        }

        var currentToy = "";
        var currentPart = "";

        foreach (var renderer in renderers) {
            var parent = renderer.transform.parent.gameObject.name;
            if (!toyDictionary.TryGetValue(parent, out var toy)) {
                toy = new List<string>();
                toyDictionary[parent] = toy;
            }
            toy.Add(renderer.gameObject.name);
            if (renderer.gameObject.activeSelf) {
                currentPart = renderer.gameObject.name;
                currentToy = parent;
            }
        }

        var possibleToys = toyDictionary.Keys.ToArray();
        var possibleParts = new string[] { "None" };
        if (toyDictionary.TryGetValue(currentToy, out var parts))
            possibleParts = parts.ToArray();

        var currentToyIndex = Array.IndexOf(possibleToys, currentToy);
        var currentPartIndex = Array.IndexOf(possibleParts, currentPart);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Toy");
        var toyIndex = EditorGUILayout.Popup(currentToyIndex, possibleToys);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Part");
        var partIndex = EditorGUILayout.Popup(currentPartIndex, possibleParts);
        EditorGUILayout.EndHorizontal();
        if (EditorGUI.EndChangeCheck()) {
            if (toyIndex < 0 || toyIndex >= possibleToys.Length)
                toyIndex = 0;
            possibleParts = new string[] { "None" };
            if (toyDictionary.TryGetValue(possibleToys[toyIndex], out parts))
                possibleParts = parts.ToArray();
            if (partIndex < 0 || partIndex >= possibleParts.Length)
                partIndex = 0;
            UpdateVisuals(renderers, possibleToys[toyIndex], possibleParts[partIndex]);
        }
    }

    private void UpdateVisuals(Renderer[] renderers, string toy, string part) {
        foreach(var renderer in renderers) {
            EditorUtility.SetDirty(renderer.gameObject);
            if (renderer.gameObject.name == part && renderer.transform.parent.gameObject.name == toy) {
                renderer.gameObject.SetActive(true);
                continue;
            }
            renderer.gameObject.SetActive(false);
        }
    }
}
