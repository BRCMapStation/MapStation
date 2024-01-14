#if MAPSTATION_DEBUG
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class StageProxyMeshMaker {
    private const string ProxyFolder = "Assets/Proxy Stages";
    [MenuItem(UIConstants.menuLabel + "/Make Stage Proxy Mesh", priority = (int)UIConstants.MenuOrder.MAKE_STAGE_PROXY_MESH)]
    private static void MakeStageProxyMesh() {
        var objects = GameObject.FindObjectsOfType<Transform>();
        Transform parentChunk = null;
        foreach (var obj in objects) {
            if (obj.name.ToLowerInvariant().EndsWith("_chunks"))
                parentChunk = obj;
        }
        if (parentChunk == null)
            parentChunk = Selection.activeTransform;
        if (parentChunk == null)
            return;
        var renderers = parentChunk.GetComponentsInChildren<MeshRenderer>();
        var maxVerts = 50000;
        var currentVerts = 0;
        var combineInstances = new List<List<CombineInstance>>();
        var currentList = new List<CombineInstance>();
        combineInstances.Add(currentList);
        foreach (var renderer in renderers) {
            if (!renderer.enabled)
                continue;
            var filter = renderer.GetComponent<MeshFilter>();
            if (filter == null)
                continue;
            if (filter.gameObject.layer != 0 && filter.gameObject.layer != 10)
                continue;
            if (filter.sharedMesh == null)
                continue;
            currentVerts += filter.sharedMesh.vertexCount;
            var combine = new CombineInstance();
            combine.mesh = filter.sharedMesh;
            combine.transform = renderer.localToWorldMatrix;
            if (renderer.gameObject.isStatic) {
                var collider = renderer.GetComponent<MeshCollider>();
                if (collider != null) {
                    combine.mesh = collider.sharedMesh;
                    combine.transform = renderer.transform.localToWorldMatrix;
                }
            }
            currentList.Add(combine);
            if (currentVerts >= maxVerts) {
                currentVerts = 0;
                currentList = new List<CombineInstance>();
                combineInstances.Add(currentList);
            }
        }

        var currentMesh = 0;
        var proxyFolder = Path.Combine(ProxyFolder, parentChunk.gameObject.scene.name);
        if (!Directory.Exists(proxyFolder))
            Directory.CreateDirectory(proxyFolder);

        var assetsToAdd = new Mesh[combineInstances.Count];
        foreach (var combList in combineInstances) {
            var result = new Mesh();
            result.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            result.CombineMeshes(combList.ToArray(), true, true);
            assetsToAdd[currentMesh] = result;
            //AssetDatabase.CreateAsset(result, Path.Combine(proxyFolder, $"{currentMesh}.asset"));
            currentMesh++;
        }

        for(var i = 0; i < assetsToAdd.Length; i++) {
            AssetDatabase.CreateAsset(assetsToAdd[i], Path.Combine(proxyFolder, $"{i}.asset"));
        }
        
    }
}
#endif