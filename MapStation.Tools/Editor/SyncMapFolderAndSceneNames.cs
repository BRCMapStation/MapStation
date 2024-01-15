using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MapStation.Common;

namespace MapStation.Tools {
    /// <summary>
    /// Detect when author renames a map's scene or its directory, and rename the other to match.
    /// </summary>
    class SyncMapFolderAndSceneNames : AssetPostprocessor
    {

        static readonly string Pattern = $"^Assets/{Regex.Escape(AssetNames.MapDirectory)}/([^/]+)/({Regex.Escape(AssetNames.SceneBasenamePrefix)})?([^/]+).unity";

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            foreach(var (i, movedAsset) in movedAssets.Pairs()) {
                var movedFromAssetPath = movedFromAssetPaths[i];
                if(movedFromAssetPath.StartsWith($"Assets/{AssetNames.MapDirectory}/") && movedFromAssetPath.EndsWith(".unity")) {
                    var fromMatch = Regex.Match(movedFromAssetPath, Pattern);
                    var toMatch = Regex.Match(movedAsset, Pattern);
                    if(fromMatch == null || toMatch == null) continue;

                    var fromDirectory = fromMatch.Groups[1].Value;
                    var fromSceneNamePrefix = toMatch.Groups[2].Value;
                    var fromSceneName = fromMatch.Groups[3].Value;
                    var toDirectory = toMatch.Groups[1].Value;
                    var toSceneNamePrefix = toMatch.Groups[2].Value;
                    var toSceneName = toMatch.Groups[3].Value;

                    if(fromDirectory == fromSceneName && fromSceneNamePrefix != "") {
                        if(fromDirectory == toDirectory && fromSceneName != toSceneName) {
                            // Rename directory to match scene
                            if(EditorUtility.DisplayDialog("Rename Map Directory",
                                "Rename map's folder to match scene?\n" +
                                "Map's scene and folder names must match.",
                                "Yes", "No"))
                            {
                                // Additionally, if the scene doesn't have the right prefix, add it.
                                if(toSceneNamePrefix == "") {
                                    AssetDatabase.RenameAsset(movedAsset, $"{AssetNames.SceneBasenamePrefix}{toSceneName}.unity");
                                }
                                Debug.Log(AssetDatabase.MoveAsset($"Assets/Maps/{fromDirectory}", $"Assets/Maps/{toSceneName}"));
                            }
                        } else if(fromDirectory != toDirectory && fromSceneName == toSceneName) {
                            // Rename scene to match directory
                            if(EditorUtility.DisplayDialog("Rename Map Scene",
                                "Rename map's scene to match folder?\n" +
                                "Map's Scene and folder names must match.",
                                "Yes", "No"))
                            {
                                Debug.Log(AssetDatabase.RenameAsset(movedAsset, $"{AssetNames.SceneBasenamePrefix}{toDirectory}.unity"));
                            }
                        }
                    }
                }
            }
        }
    }
}
