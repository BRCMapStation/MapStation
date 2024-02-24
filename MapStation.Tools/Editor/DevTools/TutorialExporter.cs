#if MAPSTATION_DEBUG
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MapStation.Tools;
using MapStation.Tools.DevTools;
using UnityEditor;
using UnityEngine;

public class TutorialExporter: MonoBehaviour
{
    [MenuItem(UIConstants.menuLabel + "/Export Tutorial", priority = (int)UIConstants.MenuOrder.EXPORT_TUTORIAL_MAP)]
    private static void ExportTutorial()
    {
        // Pack everything inside the tutorial map directory but not dependencies outside that directory
        AssetDatabase.ExportPackage(new [] {ToolAssetConstants.TutorialMapPath}, ToolAssetConstants.TutorialMapUnityPackagePath, ExportPackageOptions.Recurse);
    }
}
#endif
