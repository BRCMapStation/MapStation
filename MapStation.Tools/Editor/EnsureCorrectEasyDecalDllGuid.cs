using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MapStation.Common;
using System.IO;

namespace MapStation.Tools {
    /// <summary>
    /// Problem:
    /// If someone opens the editor project *before* they've copied EasyDecal.dll, Unity might delete the .meta file,
    /// then create a new one w/wrong GUID later. Can lead to broken graffiti.
    ///
    /// Solution:
    /// To avoid this, we rewrite the GUID when we detect it imported wrong.
    /// </summary>
    class EnsureCorrectEasyDecalDllGuid : AssetPostprocessor {

        private static readonly string AssetPath = "Assets/EasyDecal.Runtime.dll";
        private static readonly string Guid = "9fd5162cf4200a842ba7569e38d83644";

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            foreach(var importedAsset in importedAssets) {
                if (importedAsset == AssetPath) {
                    var actualGuid = AssetDatabase.AssetPathToGUID(AssetPath);
                    if (actualGuid != Guid) {
                        Debug.Log($"Fixing EasyDecal.Runtime.dll guid from {actualGuid} to {Guid}");
                        var metaFile = AssetPath + ".meta";
                        var text = File.ReadAllText(metaFile);
                        text = Regex.Replace(text, "^guid: [a-f0-9]{32}$", $"guid: {Guid}", RegexOptions.Multiline);
                        File.WriteAllText(metaFile, text);
                        AssetDatabase.Refresh();
                        return;
                    }
                }
            }
        }
    }
}
