using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MapStation.Tools {
    /// <summary>
    /// Detect when author renames a map's scene or its directory, and rename the other to match.
    /// </summary>
    class SyncMapFolderAndSceneNames : AssetPostprocessor
    {

        const string Pattern = @"^Assets/Maps/([^/]+)/(Scene-)?([^/]+).unity";

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            foreach(var (i, movedAsset) in movedAssets.Pairs()) {
                var movedFromAssetPath = movedFromAssetPaths[i];
                if(movedFromAssetPath.StartsWith("Assets/Maps/") && movedFromAssetPath.EndsWith(".unity")) {
                    var fromMatch = Regex.Match(movedFromAssetPath, Pattern);
                    var toMatch = Regex.Match(movedAsset, Pattern);
                    if(fromMatch == null || toMatch == null) continue;

                    var fromDirectory = fromMatch.Groups[1].Value;
                    var fromSceneName = fromMatch.Groups[3].Value;
                    var toDirectory = toMatch.Groups[1].Value;
                    var toSceneNamePrefix = toMatch.Groups[2].Value;
                    var toSceneName = toMatch.Groups[3].Value;
                    Debug.Log($"{fromDirectory} {fromSceneName} -> {toDirectory} {toSceneName}");

                    if(fromDirectory == fromSceneName) {
                        if(fromDirectory == toDirectory && fromSceneName != toSceneName) {
                            // Rename directory to match scene
                            if(EditorUtility.DisplayDialog("Rename Map Directory",
                                "Rename map's folder to match scene?\n" +
                                "Map's scene and folder names must match.",
                                "Yes", "No"))
                            {
                                if(toSceneNamePrefix == "") {
                                    AssetDatabase.RenameAsset(movedAsset, $"Scene-{toSceneName}.unity");
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
                                Debug.Log(AssetDatabase.RenameAsset(movedAsset, $"Scene-{toDirectory}.unity"));
                            }
                        }
                    }
                }
            }
            // Debug.Log(EditorUtility.DisplayDialog("Hello!", "Rename your stuff?\nFoo\nBar", "Yes", "No"));
            // EditorUtility.DisplayDialogComplex("Hello", "world", "ok", "cancel", "alt");
        }
    }

    static class EnumeratePairsExtensions
    {
        public static IEnumerable<KeyValuePair<int, T>> Pairs<T>(this T[] array) {
            for(int i = 0; i < array.Length; i++) {
                yield return new KeyValuePair<int, T>(i, array[i]);
            }
        }
        public static IEnumerable<KeyValuePair<int, T>> Pairs<T>(this List<T> list) {
            for(int i = 0; i < list.Count; i++) {
                yield return new KeyValuePair<int, T>(i, list[i]);
            }
        }
    }
}
