#if UNITY_EDITOR
using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.ObjectChangeEventStream;

namespace MapStation.Tools.Runtime {
    public class NavigationMeshManager : MonoBehaviour {
        [Header("Helicopter Settings")]
        public float HelicopterHeight = 8f;
        public float HelicopterMaxHeight = 20f;
        public float HelicopterMinHeight = 8f;
        public float HelicopterOuterEdges = 10f;

        public class SceneBoundingBox {
            public Vector3 Min = Vector3.zero;
            public Vector3 Max = Vector3.zero;

            public void ExtrudeEdges(float extrusion) {
                Min.x -= extrusion;
                Min.z -= extrusion;
                Max.x += extrusion;
                Max.z += extrusion;
            }

            public static SceneBoundingBox Calculate(LayerMask layerMask) {
                var bbox = new SceneBoundingBox();
                var colliders = FindObjectsOfType<Collider>();
                foreach(var collider in colliders) {
                    if ((layerMask & (1 << collider.gameObject.layer)) == 0)
                        continue;
                    if (collider.bounds.min.x < bbox.Min.x)
                        bbox.Min.x = collider.bounds.min.x;
                    if (collider.bounds.min.y < bbox.Min.y)
                        bbox.Min.y = collider.bounds.min.y;
                    if (collider.bounds.min.z < bbox.Min.z)
                        bbox.Min.z = collider.bounds.min.z;

                    if (collider.bounds.max.x > bbox.Max.x)
                        bbox.Max.x = collider.bounds.max.x;
                    if (collider.bounds.max.y > bbox.Max.y)
                        bbox.Max.y = collider.bounds.max.y;
                    if (collider.bounds.max.z > bbox.Max.z)
                        bbox.Max.z = collider.bounds.max.z;
                }
                return bbox;
            }
        }

        public bool CanAlignCopterSpawners() {
            return GetCopterNavMeshSurface() != null;
        }

        public void AlignCopterSpawners() {
            var spawners = FindObjectsOfType<CopterCopSpawner>();
            var copterColliderTf = transform.Find("CopterCollider");
            if (copterColliderTf != null)
                copterColliderTf.gameObject.SetActive(true);
            var copterSurface = GetCopterNavMeshSurface();

            foreach (var spawner in spawners) {
                var pos = spawner.transform.position;
                pos.y = HelicopterMaxHeight;
                if (Physics.Raycast(pos, Vector3.down, out var hit, Mathf.Infinity, copterSurface.layerMask))
                    spawner.transform.position = hit.point;
            }

            if (copterColliderTf != null)
                copterColliderTf.gameObject.SetActive(false);
            EditorApplication.MarkSceneDirty();
        }

        private NavMeshSurface GetCopterNavMeshSurface() {
            var surfaces = gameObject.GetComponentsInChildren<NavMeshSurface>();
            foreach (var surface in surfaces) {
                if (surface.agentTypeID == -334000983)
                    return surface;
            }
            return null;
        }

        public void GenerateNavMeshes() {
            var surfaces = gameObject.GetComponentsInChildren<NavMeshSurface>();

            GameObject copterCollider = null;
            var copterColliderTf = transform.Find("CopterCollider");
            if (copterColliderTf != null)
                copterCollider = copterColliderTf.gameObject;

            if (copterCollider != null)
                DestroyImmediate(copterCollider);

            copterCollider = new GameObject("CopterCollider");
            copterCollider.transform.SetParent(transform, false);
            var copterMeshCollider = copterCollider.AddComponent<MeshCollider>();
            var copterMeshFilter = copterCollider.AddComponent<MeshFilter>();
            copterCollider.SetActive(false);

            var builder = new CopterNavMeshBuilder();
            foreach (var surface in surfaces) {
                if (surface.agentTypeID == -334000983) {
                    var scenebbox = SceneBoundingBox.Calculate(surface.layerMask);
                    scenebbox.ExtrudeEdges(HelicopterOuterEdges);
                    builder.LayerMask = surface.layerMask;
                    builder.Origin = scenebbox.Min;
                    builder.Origin.y = HelicopterMaxHeight;
                    builder.Width = (int)((scenebbox.Max.x - scenebbox.Min.x) / builder.TileSize);
                    builder.Height = (int) ((scenebbox.Max.z - scenebbox.Min.z) / builder.TileSize);
                    builder.Build();
                    copterMeshCollider.sharedMesh = builder.Mesh;
                    copterMeshFilter.sharedMesh = builder.Mesh;
                    copterCollider.SetActive(true);
                } else
                    copterCollider.SetActive(false);
                surface.BuildNavMesh();
            }

            copterCollider.SetActive(false);
            EditorApplication.MarkSceneDirty();
        }
    }
}
#endif
