using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace MapStation.Tools.Editor {
    public class ExportToOBJ {
        [MenuItem(UIConstants.menuLabel + "/Export selection to OBJ", priority = (int)UIConstants.MenuOrder.EXPORT_TO_OBJ)]
        private static void ExportSelection() {
            var parent = Selection.activeTransform;
            if (parent == null) {
                return;
            }
            var fileName = EditorUtility.SaveFilePanel("Export .obj file", "", parent.gameObject.name, "obj");
            Export(new List<GameObject> {parent.gameObject}, fileName);
        }

        public static bool VisibleRenderersPredicate(MeshRenderer renderer) {
            if(!renderer.enabled || !renderer.gameObject.activeInHierarchy) return false;
            switch(renderer.gameObject.layer) {
                case Reptile.Layers.UI:
                case Reptile.Layers.Sun:
                case Reptile.Layers.Grinding:
                case Reptile.Layers.CameraIgnore:
                case Reptile.Layers.TriggerDetectDefault:
                case Reptile.Layers.PlayerHitbox:
                case Reptile.Layers.TriggerDetectPlayer:
                case Reptile.Layers.EnemyHitbox:
                case Reptile.Layers.DetectObstructions:
                case Reptile.Layers.VertSurface:
                case Reptile.Layers.Phone:
                case Reptile.Layers.DetectGrind:
                case Reptile.Layers.Minimap:
                case Reptile.Layers.TriggerDetectDynamic:
                case Reptile.Layers.DontUseThisLayer:
                    return false;
            }
            return true;
        }

        public static void Export(List<GameObject> gameObjects, string fileName, Func<MeshRenderer, bool> predicate = null) {
            var mesh = CombineMeshes(gameObjects, predicate);
            var objString = MakeOBJ(mesh);
            UnityEngine.Object.DestroyImmediate(mesh);
            File.WriteAllText(fileName, objString);
        }

        public static Mesh CombineMeshes(List<GameObject> gameObjects, Func<MeshRenderer, bool> predicate = null) {
            var renderers = new List<MeshRenderer>();
            foreach (var gameObject in gameObjects) {
                var childRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
                if (predicate != null) {
                    renderers.AddRange(childRenderers.Where(predicate));
                } else {
                    renderers.AddRange(childRenderers);
                }
            }
            var combinedMesh = new Mesh();
            combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            var instances = new List<CombineInstance>();
            foreach(var renderer in renderers) {
                var filter = renderer.GetComponent<MeshFilter>();
                if (filter == null)
                    continue;
                if (filter.sharedMesh == null)
                    continue;

                for(var j = 0; j < renderer.sharedMaterials.Length; j++) {
                    instances.Add(new CombineInstance {
                        mesh = filter.sharedMesh,
                        subMeshIndex = renderer.subMeshStartIndex + j,
                        transform = renderer.localToWorldMatrix
                    });
                }
            }

            combinedMesh.CombineMeshes(instances.ToArray(), true, true);
            return combinedMesh;
        }

        public static string MakeOBJ(Mesh mesh) {
            var sb = new StringBuilder();
            var verts = mesh.vertices;
            var uvs = mesh.uv;
            var normals = mesh.normals;
            for(var i=0;i<verts.Length;i++) {
                sb.AppendLine($"v {-verts[i].x} {verts[i].y} {verts[i].z}");
                sb.AppendLine($"vt {uvs[i].x} {uvs[i].y}");
                sb.AppendLine($"vn {-normals[i].x} {normals[i].y} {normals[i].z}");
            }
            var faces = mesh.triangles;
            for(var i=0;i<faces.Length;i+=3) {
                var f1 = faces[i + 2] + 1;
                var f2 = faces[i + 1] + 1;
                var f3 = faces[i] + 1;
                sb.AppendLine($"f {f1}/{f1}/{f1} {f2}/{f2}/{f2} {f3}/{f3}/{f3}");
            }
            return sb.ToString();
        }
    }
}
