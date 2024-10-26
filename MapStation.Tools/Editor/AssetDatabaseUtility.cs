using System.IO;
using UnityEditor;
using UnityEngine;

namespace MapStation.Tools {
    public static class AssetDatabaseUtility {
        /// <summary>
        /// Given the path to a file you want to create, ensures the folder
        /// to contain that file exists.
        /// </summary>
        public static void EnsureFolderExistsForFile(string path) {
            var dirToCreate = Path.GetDirectoryName(path);
            var parent = Path.GetDirectoryName(dirToCreate);
            var name = Path.GetFileName(dirToCreate);
            if(!AssetDatabase.IsValidFolder(dirToCreate)) {
                Debug.Log($"Creating {parent} / {name}");
                AssetDatabase.CreateFolder(parent, name);
            }
        }

        public static void EnsureFolderExistsForFileUsingCsharpFilesystemApi(string path) {
            var dirToCreate = Path.GetDirectoryName(path);
            var parent = Path.GetDirectoryName(dirToCreate);
            var name = Path.GetFileName(dirToCreate);
            if (!Directory.Exists(dirToCreate)) {
                EnsureFolderExistsForFileUsingCsharpFilesystemApi(dirToCreate);
                Directory.CreateDirectory(dirToCreate);
            }
        }
    }
}
