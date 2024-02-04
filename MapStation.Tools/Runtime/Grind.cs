using System.Collections.Generic;
using Reptile;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using MapStation.Common;

namespace MapStation.Components {
    [ExecuteInEditMode]
    [StripFromAssetBundleScene]
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
        public Transform NodesContainer;
        [HideInInspector]
        [SerializeField]
        public Transform LinesContainer;
        [HideInInspector]
        [SerializeField]
        public Transform SplinesContainer;

        [HideInInspector]
        [SerializeField]
        public GrindPath GrindPath;

        [NonReorderable]
        public List<GrindNode> nodes = new();
        [NonReorderable]
        public List<GrindLine> lines = new();

        #if UNITY_EDITOR

        /// Return a nice, numeric name for a new node or line GameObject, based on
        /// all the names that have already been taken.  Names will count up from 1,
        /// but this logic won't break if you decide to rename your GameObjects.
        private IEnumerator<string> generateNamesForNewNodesOrLines<T>(List<T> components) where T : MonoBehaviour {
            int name = components.Count;
            foreach(var c in components) {
                if (int.TryParse(c.name, out int i)) {
                    name = Math.Max(name, i);
                }
            }
            while(true) {
                name++;
                yield return name.ToString();
            }
        }

        public void AddNode(GrindNode branchFromNode = null)
        {
            AddNodes(new GrindNode[] {branchFromNode});
        }

        public void AddNodes(GrindNode[] branchFromNodes) {
            Undo.RegisterCompleteObjectUndo(gameObject, "");

            List<GameObject> newNodeGameObjects = new();
            var nodeNames = generateNamesForNewNodesOrLines(nodes);
            foreach(var item in branchFromNodes) {
                var branchFromNode = item != null ? item : nodes.LastOrDefault();
                var cloneFromGameObject = branchFromNode != null ? branchFromNode.gameObject : NodePrefab;
                var parent = branchFromNode != null ? branchFromNode.transform.parent : NodesContainer;

                var newNode = Instantiate(cloneFromGameObject, parent).GetComponent<GrindNode>();

                // Undoing this will trigger newNode's `OnDestroy()`, which destroys
                // attached lines.
                // Null out all lines to ensure that `OnDestroy` will not destroy the wrong line.
                for(int i = 0; i < newNode.grindLines.Count(); i++) {
                    newNode.grindLines[i] = null;
                }
                Undo.RegisterCreatedObjectUndo(newNode.gameObject, "");

                if(branchFromNode != null)
                    newNode.transform.position = branchFromNode.transform.position + Preferences.instance.grinds.newNodeOffset;
                else
                    newNode.transform.position = transform.position;

                newNode.name = nodeNames.TakeNext();

                if(branchFromNode != null) {
                    Undo.RegisterCompleteObjectUndo(branchFromNode.gameObject, "");
                    Undo.RegisterCompleteObjectUndo(branchFromNode, "");
                    AddLine(branchFromNode, newNode, branchFromNode.grindLines.Find(x => x != null));
                }
                newNodeGameObjects.Add(newNode.gameObject);
                nodes.Add(newNode);
            }

            GrindUtils.autoSelectIfEnabled(newNodeGameObjects);
            Undo.SetCurrentGroupName("Add grind node(s)");
        }

        public void AddLine(GrindNode n0, GrindNode n1, GrindLine cloneFromLine)
        {
            // TODO teach this method to track undo! Though informal testing shows it already works?
            var cloneFromGameObject = cloneFromLine != null ? cloneFromLine.gameObject : LinePrefab;
            var parent = cloneFromLine != null ? cloneFromLine.transform.parent : LinesContainer;

            var newLine = Instantiate(cloneFromGameObject, parent).GetComponent<GrindLine>();

            // TODO was sterilizing for Undo, but not necessary here.
            for(int i = 0; i < newLine.nodes.Count(); i++) {
                newLine.nodes[i] = null;
            }
            Undo.RegisterCreatedObjectUndo(newLine.gameObject, "");

            newLine.name = generateNamesForNewNodesOrLines(lines).TakeNext();
            
            n0.AddLine(newLine);
            n1.AddLine(newLine);

            newLine.nodes[0] = n0;
            newLine.nodes[1] = n1;

            newLine.RebuildWithRedDebugShape();

            lines.Add(newLine);
        }

        void OnValidate() {
            if(EditorUtility.IsPersistent(this) || MapBuilderStatus.IsBuilding) return;
            // Unpack prefab as soon as it's added to the scene.
            // This allows deleting the prefab's default 2x nodes and grindline without
            // breaking the prefab or prompting the user "children of a prefab instance cannot be deleted..."
            if(PrefabUtility.IsAnyPrefabInstanceRoot(gameObject)) {
                PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            }
        }

        #endif
    }
}