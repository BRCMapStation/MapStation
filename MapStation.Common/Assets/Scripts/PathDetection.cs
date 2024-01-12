using Microsoft.Win32;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MapStation.Common {
    public class PathDetection {
        // Must match setup-implementation.ps1!
        public const string RegistryKey = @"HKEY_CURRENT_USER\Software\BRCMapStation\MapStation";

        // Example: C:\Users\cspot\AppData\Roaming\Thunderstore Mod Manager\DataFolder\BombRushCyberfunk\profiles\Default\BepInEx
        public const string RegistryValueBepInExProfile = "BepInExProfile";

        // Example: C:\Program Files (x86)\Steam\steamapps\common\BombRushCyberfunk
        public const string RegistryValueBRCPath = "BRCPath";
        
        public static string GetBepInExProfileInRegistry() {
            return Registry.GetValue(RegistryKey, RegistryValueBepInExProfile, "") as string;
        }

        public static void SetBepInExProfileInRegistry(string path) {
            Registry.SetValue(RegistryKey, RegistryValueBepInExProfile, path);
        }

        public static string GetBRCPathInRegistry() {
            return Registry.GetValue(RegistryKey, RegistryValueBRCPath, "") as string;
        }

        public static void SetBRCPathInRegistry(string path) {
            Registry.SetValue(RegistryKey, RegistryValueBRCPath, path);
        }
    }
}
