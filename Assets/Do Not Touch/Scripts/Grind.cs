using System.Collections;
using System.Collections.Generic;
using Reptile;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class Grind : MonoBehaviour
{
    public List<Reptile.GrindNode> nodes = new();
    public List<Reptile.GrindLine> lines = new();

    public void ListNodes()
    {
        nodes.Clear();
        var sortedNodes = GetComponentsInChildren<GrindNode>().OrderBy(go => int.Parse(go.name));
        nodes.AddRange(sortedNodes);
    }


    public void ListLines()
    {
        lines.Clear();
        var sortedLines = GetComponentsInChildren<GrindLine>().OrderBy(go => int.Parse(go.name));
        lines.AddRange(sortedLines);
    }

    public void AddNode()
    {
        var lastNode = nodes.Last();
        var newNode = Instantiate(lastNode.gameObject, lastNode.transform.parent);
#if UNITY_EDITOR
        Undo.RegisterCreatedObjectUndo(newNode, "New Node");
#endif
        newNode.transform.position = lastNode.transform.position + lastNode.transform.forward;
        newNode.name = (int.Parse(lastNode.name) + 1).ToString();

        AddLine(lastNode, newNode.GetComponent<GrindNode>());
    }

    public void RemoveNode()
    {
        // Get the last node and line
        var lastNode = nodes.Last();
        var lastLine = lines.Last();
        nodes.Remove(lastNode);
        lines.Remove(lastLine);
        GameObject.DestroyImmediate(lastNode.gameObject);
        GameObject.DestroyImmediate(lastLine.gameObject);
    }

    public void AddLine(GrindNode lastNode, GrindNode newNode)
    {
        var lastLine = lastNode.grindLines[0];
        if (lastNode.grindLines.Count == 2)
            lastLine = lastNode.grindLines[1];

        var newLine = Instantiate(lastLine.gameObject, lastLine.transform.parent);
#if UNITY_EDITOR
        Undo.RegisterCreatedObjectUndo(newLine, "New Line");
#endif
        newLine.name = (int.Parse(lastLine.name) + 1).ToString();
        var newGrindLine = newLine.GetComponent<GrindLine>();
        newNode.grindLines[0] = lastLine;
        if (newNode.grindLines[0])
        {
            newNode.grindLines[0] = newGrindLine;
        }
        else
            newNode.grindLines.Add(newGrindLine);

        if (lastNode.grindLines.Count == 2)
        {
            lastNode.grindLines[1] = newGrindLine;
        }
        else
            lastNode.grindLines.Add(newGrindLine);

        newGrindLine.nodes[0] = lastNode;
        newGrindLine.nodes[1] = newNode;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
