using System;
using System.Collections.Generic;
using UnityEngine;
using MapStation.Common;
using UnityEngine.Rendering;
using UnityEditor;
using System.Text.RegularExpressions;
using MapStation.Tools;

namespace MapStation.Tools {
    [CreateAssetMenu(fileName = "Properties", menuName = UIConstants.menuLabel + "/Map Properties", order = 1)]
    public class MapPropertiesScriptableObject : ScriptableObject {
        /// <summary>
        /// This is extracted as properties.json we bundle into each .brcmap
        /// </summary>
        public MapProperties properties = new MapProperties();

        /// <summary>
        /// Name of the map's Thunderstore mod. Can only use letters, numbers, and underscores.
        /// </summary>
        public string thunderstoreName;
        public bool setThunderstoreNameAutomatically = true;
        public string description;
        public string website;

#if UNITY_EDITOR
        /// <summary>
        /// Ensure automatically-set values are correct.
        /// This runs in inspector and right before building maps.
        /// </summary>
        public void SyncAutomaticFields(BaseMapDatabaseEntry map) {
            if(properties.internalName != map.Name) {
                properties.internalName = map.Name;
                EditorUtility.SetDirty(this);
            }
            if(setThunderstoreNameAutomatically) {
                var newName = thunderstoreNameFromDisplayName(properties.displayName);
                if(newName != thunderstoreName) {
                    thunderstoreName = newName;
                    EditorUtility.SetDirty(this);
                }
            }
        }

        private string thunderstoreNameFromDisplayName(string displayName) {
            return Regex.Replace(Regex.Replace(displayName, " ", "_"), @"[^\da-zA-Z_]", "");
        }
#endif
    }
}
