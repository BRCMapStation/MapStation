using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MapStation.Tools.Editor {
    internal static class EditorHelper {
        public static void MakeDocsButton(string page) {
            if (GUILayout.Button("View in Docs")) {
                Application.OpenURL(ToolConstants.WikiRootAddress + page);
            }
        }
    }
}
