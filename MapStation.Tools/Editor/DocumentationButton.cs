using MapStation.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class DocumentationButton {
    [MenuItem(UIConstants.menuLabel + "/Documentation", priority = (int) UIConstants.MenuOrder.DOCUMENTATION)]
    private static void OpenDocumentation() {
        Application.OpenURL(ToolConstants.WikiRootAddress);
    }
}
