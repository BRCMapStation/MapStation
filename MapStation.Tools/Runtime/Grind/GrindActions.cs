
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MapStation.Common.Doctor;
using MapStation.Components;
using Reptile;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;
using Debug = UnityEngine.Debug;

/// <summary>
/// Implement all mutations which can be performed on a Grind, its nodes and lines.
/// Tracks Undo.
/// 
/// Undo tracking and properly updating grinds often spans mutations across multiple
/// Components, so I think it's helpful to centralize the logic here.
/// </summary>
public static class GrindActions {

    /// AddLine(Grind, NodeA, NodeB, CloneFromLine?)
    /// RemoveLine(Grind, Line)
    /// RemoveNode(Grind, Node)
    /// SplitLine(Grind, Line)
    /// MergeNodes(Grind, NodeA, NodeB)
    /// ConvertLineToSpline(Grind, Line)
    /// ConvertLineToLinear(Grind, SplineLine)
    /// MergeGrinds(GrindA, GrindB)
    /// 

    const string GrindNodePrefabPath = "Packages/com.brcmapstation.tools/Assets/Prefabs/GrindNodePrefab.prefab";
    const string GrindLinePrefabPath = "Packages/com.brcmapstation.tools/Assets/Prefabs/GrindLinePrefab.prefab";

    /// <summary>
    /// 
    /// Repair the Grind data structure as much as possible so subsequent code
    /// is error-free.
    /// 
    /// We intentionally allow mappers to edit grinds using Unity's tools.
    /// This means the data structure might break: nodes moved to a different
    /// grind, deleted nodes, lines and nodes not referencing each other, etc.
    /// 
    /// SyncReferences(Grind)
    ///     re-discover all nodes and lines
    ///     remove nulls
    ///     re-link all nodes and lines to GrindPath and Grind
    ///     Warn on duplicate GrindPath or duplicate Grind
    ///     
    /// </summary>
    static void SyncReferences(Grind grind) {
        var undoLabel = "GrindActions.SyncReferences";

        void ensurePrefab(ref GameObject prefab, string path) {
            if(prefab == null) {
                prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if(prefab == null) {
                    Debug.LogError($"Could not find prefab {GrindNodePrefabPath}");
                }
            }
        }

        Undo.RecordObject(grind, undoLabel);
        ensurePrefab(ref grind.NodePrefab, GrindNodePrefabPath);
        ensurePrefab(ref grind.LinePrefab, GrindLinePrefabPath);

        // Ensure single GrindPath child
        var paths = grind.GetComponentsInChildren<GrindPath>();
        if(paths.Length > 1) {
            throw new System.Exception($"Grind {Doctor.GetGameObjectPath(grind.gameObject)} has multiple GrindPaths; this should never happen; Grind should be recreated.");
        } else if(paths.Length == 0) {
            var path = ObjectFactory.CreateGameObject("GrindPath", typeof(GrindPath));
            Undo.SetTransformParent(path.transform, grind.transform, undoLabel);
            Undo.RecordObject(grind, undoLabel);
            grind.GrindPath = path.GetComponent<GrindPath>();
        } else {
            Undo.RecordObject(grind, undoLabel);
            grind.GrindPath = paths[0];
        }

        void ensureContainer(ref Transform container, string name, Transform parent) {
            if(container == null) {
                container = ObjectFactory.CreateGameObject(name).transform;
                Undo.SetTransformParent(container, parent, undoLabel);
            }
        }

        ensureContainer(ref grind.NodesContainer, "Nodes", grind.GrindPath.transform);
        ensureContainer(ref grind.LinesContainer, "Lines", grind.GrindPath.transform);
        // Don't create spline stuff because it will confuse users, will believe splines are usable
        // ensureContainer(ref grind.SplineParent, "SplineLines", grind.transform);

        void syncListOfComponents<T>(List<T> list, T[] components) where T : Component {
            // UNITY COMPLEXITY:
            // If you delete an object, then undo the deletion,
            // Unity creates a new object with same ID as the old one.
            // == will return true(?) but the old reference is still destroyed and cannot be used!

            if(components.Length != list.Count) goto Sync;
            for(var i = 0; i < components.Length; i++) {
                if(!ReferenceEquals(components[i], list[i])) goto Sync;
            }
            return;

            Sync:
            list.Clear();
            list.AddRange(components);
        }

        Undo.RecordObject(grind, undoLabel);
        syncListOfComponents(grind.nodes, grind.GetComponentsInChildren<GrindNode>(includeInactive: true));
        syncListOfComponents(grind.lines, grind.GetComponentsInChildren<GrindLine>(includeInactive: true));

        foreach(var line in grind.lines) {
            Undo.RecordObject(line, undoLabel);
            line.PathReference = grind.GrindPath;
            if(line.nodes.Length != 2) {
                var old = line.nodes;
                line.nodes = new GrindNode[2];
                if(old.Length >= 1) line.nodes[0] = old[0];
                if(old.Length >= 2) line.nodes[1] = old[1];
            }

            // SKIPPED Check that is connected to 2x nodes, destroy otherwise
            // Skipping in case there are weird cases where someone wants to manually rewire stuff (??)

            void syncNodeToLine(ref GrindNode n) {
                // Replace unity's fake null with real null
                if(n == null) {
                    n = null;
                    return;
                }
                // Check that node is from the same Grind, detach otherwise
                if(!grind.nodes.Contains(n)) {
                    Undo.RecordObject(line, undoLabel);
                    n = null;
                }
                // Ensure that node connects to this line
                if(!n.grindLines.Contains(line)) {
                    Undo.RecordObject(n, undoLabel);
                    n.grindLines.Add(line);
                }
            }
            syncNodeToLine(ref line.nodes[0]);
            syncNodeToLine(ref line.nodes[1]);
        }

        foreach(var node in grind.nodes) {
            Undo.RecordObject(node, undoLabel);
            node.PathReference = grind.GrindPath;

            // Reverse order so we can remove items without affecting next index
            for(var i = node.grindLines.Count - 1; i >= 0; i--) {
                var line = node.grindLines[i];
                // Remove nulls
                if(line == null) {
                    node.grindLines.RemoveAt(i);
                    continue;
                }
                // Check that each line is part of the same grind
                if(!grind.lines.Contains(line)) {
                    node.grindLines.RemoveAt(i);
                    continue;
                }
                // If line does not mutually connect to this node, but has null endpoint,
                // attach it to us.  Otherwise detach from the line.
                if(line.n0 != node && line.n1 != node) {
                    Undo.RecordObject(line, undoLabel);
                    if(ReferenceEquals(line.n0, null)) {
                        line.n0 = node;
                    } else if(ReferenceEquals(line.n1, null)) {
                        line.n1 = node;
                    } else {
                        node.grindLines.RemoveAt(i);
                    }
                    continue;
                }
            }
        }
    }

    public static void Repair(Grind grind) {
        SyncReferences(grind);
        foreach(var line in grind.lines) {
            line.RebuildWithRedDebugShape();
        }
    }

    ///
    /// RebuildDebugShape
    ///     Sync line's debug shape to match endpoint nodes and line config
    ///     

    public static void AddNodes(Grind grind, GrindNode[] branchFromNodes) {
        SyncReferences(grind);
        grind.AddNodes(branchFromNodes);
    }

    public static void AddNode(Grind grind) {
        SyncReferences(grind);
        grind.AddNode();
    }

    public static void RemoveLastNode(Grind grind) {
        SyncReferences(grind);
        Undo.RegisterCompleteObjectUndo(grind.gameObject, "");

        // Get the last node, to be removed
        var lastNode = grind.nodes.LastOrDefault();
        if(lastNode == null) {
            Debug.LogError("Grind has no nodes to remove.");
        }
        grind.nodes.Remove(lastNode);

        GrindNode selectThisNodeAfterRemoval = null;

        // Detach all connected lines from their other node, tracking undo.
        foreach(var l in lastNode.grindLines) {
            grind.lines.Remove(l);
            var otherNode = l.n0 == lastNode ? l.n1 : l.n0;
            if(otherNode != null) {
                selectThisNodeAfterRemoval = otherNode;
                Undo.RegisterCompleteObjectUndo(otherNode.gameObject, "");
                otherNode.RemoveLine(l);
            }
        }

        // Destroy lines
        foreach(var l in new List<GrindLine>(lastNode.grindLines)) {
            Undo.DestroyObjectImmediate(l.gameObject);
        }

        // Destroy node
        Undo.DestroyObjectImmediate(lastNode.gameObject);

        if(selectThisNodeAfterRemoval != null) {
            GrindUtils.autoSelectIfEnabled(selectThisNodeAfterRemoval.gameObject);
        }

        Undo.SetCurrentGroupName("Remove grind node");
    }

    public static bool CanJoinNodes(GrindNode n0, GrindNode n1) {
        return n0.Grind != null && n0.Grind == n1.Grind && !n0.IsConnectedTo(n1);
    }

    public static void JoinNodes(GrindNode n0, GrindNode n1) {
        SyncReferences(n0.Grind);
        n0.Grind.AddLine(n0, n1, n0.grindLines.Find(x => x != null));

    }

    public static void OrientNodesUpward(GrindNode[] nodes) {
        foreach(var node in nodes) {
            Undo.RecordObject(node.transform, "Orient GrindNode(s) upward");
            node.transform.localRotation = Quaternion.identity;
        }
    }

    public static void OrientNodesDownward(GrindNode[] nodes) {
        foreach(var node in nodes) {
            Undo.RecordObject(node.transform, "Orient GrindNode(s) downward");
            node.transform.localRotation = Quaternion.FromToRotation(Vector3.forward, Vector3.back);
        }
    }

    public static void RemoveLineFromNode(GrindLine line, GrindNode node) {
        Undo.RegisterCompleteObjectUndo(node, Undo.GetCurrentGroupName());
        node.RemoveLine(line);
    }
}