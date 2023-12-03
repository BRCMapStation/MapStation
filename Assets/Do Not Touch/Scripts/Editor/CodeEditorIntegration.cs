using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CsprojPostprocessorWindow : EditorWindow {
    const string WindowLabel = "Csproj Postprocessor";

    [MenuItem(Constants.menuLabel + "/" + Constants.experimentsSubmenuLabel + "/" + WindowLabel, priority = Constants.experimentsSubmenuPriority)]
    private static void ShowMyEditor() {
        EditorWindow wnd = GetWindow<CsprojPostprocessorWindow>();
        wnd.titleContent = new GUIContent(WindowLabel);
    }

    private bool applyModifications_ = false;

    private void OnEnable() {
        // On domain reload, restore static state from serialized instance state
        CSProjPostprocessor.applyModifications = applyModifications_;
    }

    private void OnGUI() {
        EditorGUILayout.HelpBox(
            "Tweaks .csproj files emitted by Unity, for compatibility with VSCode.\n" + 
            "Or maybe cspotcode doesn't know what he's doing.",
            MessageType.Info
        );
        CSProjPostprocessor.applyModifications = applyModifications_ = GUILayout.Toggle(applyModifications_, "Apply modifications");
    }
}

public class CSProjPostprocessor : AssetPostprocessor
{
    internal static bool applyModifications = false;

    private static string OnGeneratedCSProject(string path, string contents) {
        if(applyModifications) {
            contents = contents.Replace("<ProjectCapability Remove=\"AssemblyReferences\" />", "<!-- <ProjectCapability Remove=\"AssemblyReferences\" /> -->");
            contents = contents.Replace("<TargetFramework>netstandard2.1</TargetFramework>", "<TargetFramework>net46</TargetFramework>");
            contents = contents.Replace("<LangVersion>9.0</LangVersion>", "<LangVersion>latest</LangVersion>");
        }
        return contents;
    }
}