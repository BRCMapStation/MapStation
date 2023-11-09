using System.Collections.Generic;
using Reptile;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

public class Grind : MonoBehaviour
{
    // These are saved in the prefab but hidden in the inspector, because UX.
    // If these references break, you can remove the Hide annotations to reveal
    // in inspector, reattach, then uncomment annotations.
    [HideInInspector]
    [SerializeField]
    public GameObject NodePrefab;
    [HideInInspector]
    [SerializeField]
    public GameObject LinePrefab;
    [HideInInspector]
    [SerializeField]
    private Transform NodeParent;
    [HideInInspector]
    [SerializeField]
    private Transform LineParent;
    [HideInInspector]
    [SerializeField]
    private Transform SplineParent;

    [NonReorderable]
    public List<GrindNode> nodes = new();
    [NonReorderable]
    public List<GrindLine> lines = new();

    #if UNITY_EDITOR

    public void ListNodes()
    {
        nodes.Clear();
        var sortedNodes = GetComponentsInChildren<GrindNode>();//.OrderBy(go => int.Parse(go.name));
        nodes.AddRange(sortedNodes);
    }


    public void ListLines()
    {
        lines.Clear();
        var sortedLines = GetComponentsInChildren<GrindLine>();//.OrderBy(go => int.Parse(go.name));
        lines.AddRange(sortedLines);
    }

    /// Return a nice, numeric name for a new node or line GameObject, based on
    /// all the names that have already been taken.  Names will count up from 1,
    /// but this logic won't break if you decide to rename your GameObjects.
    private string getNameForNewNodeOrLine<T>(List<T> components) where T : MonoBehaviour {
        int name = components.Count();
        foreach(var c in components) {
            if (int.TryParse(c.name, out int i))
                name = Math.Max(name, i);
        }
        name++;
        return name.ToString();
    }

    public void AddNode(GrindNode branchFromNode = null)
    {
        Undo.IncrementCurrentGroup();
        Undo.RegisterCompleteObjectUndo(gameObject, "");

        if(branchFromNode == null) branchFromNode = nodes.LastOrDefault();
        var cloneFromGameObject = branchFromNode != null ? branchFromNode.gameObject : NodePrefab;
        var parent = branchFromNode != null ? branchFromNode.transform.parent : NodeParent;

        var newNode = Instantiate(cloneFromGameObject, parent).GetComponent<GrindNode>();

        // Undoing this will trigger newNode's `OnDestroy()`, which destroys
        // attached lines.
        // Null out all lines to ensure that `OnDestroy` will not destroy the wrong line.
        for(int i = 0; i < newNode.grindLines.Count(); i++) {
            newNode.grindLines[i] = null;
        }
        Undo.RegisterCreatedObjectUndo(newNode.gameObject, "");

        if(branchFromNode != null)
            newNode.transform.position = branchFromNode.transform.position + branchFromNode.transform.forward;
        else
            newNode.transform.position = transform.position;

        newNode.name = getNameForNewNodeOrLine(nodes);

        Undo.RegisterCompleteObjectUndo(branchFromNode.gameObject, "");
        Undo.RegisterCompleteObjectUndo(branchFromNode, "");
        AddLine(branchFromNode, newNode, branchFromNode.grindLines.Find(x => x != null));

        GrindUtils.autoSelectIfEnabled(newNode.gameObject);
        Undo.SetCurrentGroupName("Add grind node");
    }

    public void RemoveNode()
    {
        Undo.IncrementCurrentGroup();
        Undo.RegisterCompleteObjectUndo(gameObject, "");

        // Get the last node, to be removed
        var lastNode = nodes.LastOrDefault();
        if(lastNode == null) {
            Debug.LogError("Grind has no nodes to remove.");
        }
        nodes.Remove(lastNode);

        GrindNode selectThisNodeAfterRemoval = null;

        // Detach all connected lines from their other node, tracking undo.
        foreach(var l in lastNode.grindLines) {
            lines.Remove(l);
            var otherNode = l.n0 == lastNode ? l.n1 : l.n0;
            if(otherNode != null) {
                selectThisNodeAfterRemoval = otherNode;
                Undo.RegisterCompleteObjectUndo(otherNode.gameObject, "");
                otherNode.RemoveLine(l);
            }
        }

        // Destroy lines
        foreach(var l in lastNode.grindLines) {
            Undo.DestroyObjectImmediate(l.gameObject);
        }

        // Destroy node
        Undo.DestroyObjectImmediate(lastNode.gameObject);

        if(selectThisNodeAfterRemoval != null) {
            GrindUtils.autoSelectIfEnabled(selectThisNodeAfterRemoval.gameObject);
        }

        Undo.SetCurrentGroupName("Remove grind node");
    }

    public void AddLine(GrindNode n0, GrindNode n1, GrindLine cloneFromLine)
    {
        var cloneFromGameObject = cloneFromLine != null ? cloneFromLine.gameObject : LinePrefab;
        var parent = cloneFromLine != null ? cloneFromLine.transform.parent : LineParent;

        var newLine = Instantiate(cloneFromGameObject, parent).GetComponent<GrindLine>();

        // TODO was sterilizing for Undo, but not necessary here.
        for(int i = 0; i < newLine.nodes.Count(); i++) {
            newLine.nodes[i] = null;
        }
        Undo.RegisterCreatedObjectUndo(newLine.gameObject, "");

        newLine.name = getNameForNewNodeOrLine(lines);
        
        n0.AddLine(newLine);
        n1.AddLine(newLine);

        newLine.nodes[0] = n0;
        newLine.nodes[1] = n1;

        newLine.RebuildWithRedDebugShape();
    }

    void OnValidate() {
        // Unpack prefab as soon as it's added to the scene.
        // This allows deleting the prefab's default 2x nodes and grindline without
        // breaking the prefab or prompting the user "children of a prefab instance cannot be deleted..."
        if(PrefabUtility.IsAnyPrefabInstanceRoot(gameObject)) {
            PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }
    }
    #endif
}
