using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class StageProxyMeshMaker {
    private const string ProxyFolder = "Assets/Proxy Stages";
    [MenuItem("BRC/Make Stage Proxy Mesh")]
    private static void MakeStageProxyMesh() {
        var objects = GameObject.FindObjectsOfType<Transform>();
        Transform parentChunk = null;
        foreach (var obj in objects) {
            if (obj.name.ToLowerInvariant().EndsWith("_chunks"))
                parentChunk = obj;
        }
        if (parentChunk == null)
            return;
        var renderers = parentChunk.GetComponentsInChildren<MeshRenderer>();
        var combine = new CombineInstance[renderers.Length];
        for (var i = 0; i < renderers.Length; i++) {
            var filter = renderers[i].GetComponent<MeshFilter>();
            if (filter == null)
                continue;
            if (filter.gameObject.layer != 0)
                continue;
            combine[i].mesh = filter.sharedMesh;
            combine[i].transform = renderers[i].localToWorldMatrix;
        }

        var result = new Mesh();
        result.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        result.CombineMeshes(combine, true, true);
        if (!Directory.Exists(ProxyFolder))
            Directory.CreateDirectory(ProxyFolder);
        AssetDatabase.CreateAsset(result, Path.Combine(ProxyFolder, $"{parentChunk.gameObject.scene.name}.asset"));
    }
}
