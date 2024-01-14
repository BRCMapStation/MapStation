using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class ExportToOBJ {
    [MenuItem(UIConstants.menuLabel + "/Export selection to OBJ", priority = (int)UIConstants.MenuOrder.EXPORT_TO_OBJ)]
    private static void Export() {
        var parent = Selection.activeTransform;
        if (parent == null)
            return;
        var renderers = parent.GetComponentsInChildren<MeshRenderer>();
        if (renderers.Length <= 0)
            return;
        var fileName = EditorUtility.SaveFilePanel("Export .obj file", "", parent.gameObject.name, "obj");
        if (string.IsNullOrEmpty(fileName))
            return;
        var combinedMesh = new Mesh();
        combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        var instances = new CombineInstance[renderers.Length];
        for(var i=0;i<renderers.Length;i++) {
            var filter = renderers[i].GetComponent<MeshFilter>();
            if (filter == null)
                continue;
            if (filter.sharedMesh == null)
                continue;
            instances[i].mesh = filter.sharedMesh;
            instances[i].transform = renderers[i].localToWorldMatrix;
        }
        combinedMesh.CombineMeshes(instances, true, true);
        var objString = MakeOBJ(combinedMesh);
        UnityEngine.Object.DestroyImmediate(combinedMesh);
        File.WriteAllText(fileName, objString);
    }

    private static string MakeOBJ(Mesh mesh) {
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
