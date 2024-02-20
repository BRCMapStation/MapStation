using System.IO;
using Reptile;
using UnityEditor;
using UnityEngine;

class CustomImportProcessor : AssetPostprocessor {
    
    // void OnPostprocessGameObjectWithUserProperties(GameObject go, string[] names, System.Object[] values) {
    //     ModelImporter importer = (ModelImporter)assetImporter;
    //     var asset_name = Path.GetFileName(importer.assetPath);
    //     Debug.LogFormat("OnPostprocessGameObjectWithUserProperties(go = {0}) asset = {1} path = {2}", go.name, asset_name, importer.assetPath);
    //     Vector3 vec3 = Vector3.zero;
    //     for (int i = 0; i < names.Length; i++) {
    //         var name = names[i];
    //         var val = values[i];
    //         Debug.Log($"{name}={val}");
    //     }
    //     var teleport = go.AddComponent<Teleport>();
    //     teleport.teleportTo = go.transform;
    // }
    //
    // void OnPostprocessMeshHierarchy(GameObject go) {
    //     Debug.Log("OnPostprocessMeshHierarchy");
    //     var teleport = go.AddComponent<Teleport>();
    //     teleport.teleportTo = go.transform;
    //
    // }
    // void OnPostprocessModel(GameObject go) {
    //     Debug.Log("OnPostprocessModel");
    //     var teleport = go.AddComponent<Light>();
    //
    // }
    // void OnPostprocessPrefab(GameObject go) {
    //     Debug.Log("OnPostprocessPrefab");
    //     var teleport = go.AddComponent<Teleport>();
    //     teleport.teleportTo = go.transform;
    //
    // }
}
