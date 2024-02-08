using System.IO;
using UnityEditor;
using UnityEngine;

namespace MapStation.Tools.DevTools {
    public class InstallGit {
        [MenuItem(UIConstants.menuLabel + "/" + UIConstants.experimentsSubmenuLabel + "/Install Git", priority = (int) UIConstants.MenuOrder.EXPERIMENTS)]
        private static void Install() {
            var rootFolder = Directory.GetCurrentDirectory();
            var script = Path.Combine(rootFolder, "scripts", "install-git.ps1");
            var process = PowershellUtil.RunScript(script);
            process.WaitForExit();
            if (process.ExitCode != 0) {
                Debug.LogError("Git installation failed");
            }
        }
    }
}
