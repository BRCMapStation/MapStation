#if MAPSTATION_DEBUG
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// HACK: Patch Unity's Visual Studio integration to open our root MapStation.sln instead of the editor's generated
/// MapStation.Editor.sln
/// 
/// This is a convenience: when clicking errors in Unity Editor, they will open in the correct Visual Studio window.
/// </summary>
static class VisualStudioEditorPatch {
    private const string FindInVisualStudioEditorCs =
        "\t\t\tvar solution = GetOrGenerateSolutionFile(generator);\n" +
        "\t\t\treturn installation.Open(path, line, column, solution);";
    private const string ReplaceInVisualStudioEditorCs =
        "\t\t\tvar solution = GetOrGenerateSolutionFile(generator);\n" +
        "\t\t\tsolution = Path.Join(Path.GetDirectoryName(Path.GetDirectoryName(solution)), \"MapStation.sln\");\n" +
        "\t\t\treturn installation.Open(path, line, column, solution);";

    [InitializeOnLoadMethod]
    private static void ApplyPatch() {
        foreach(var pkg in UnityEditor.PackageManager.PackageInfo.GetAllRegisteredPackages()) {  
            if(pkg.name == "com.unity.ide.visualstudio") {
                var path = Path.Join(Path.Join(Path.GetDirectoryName(Application.dataPath), "Library/PackageCache", pkg.packageId), "Editor/VisualStudioEditor.cs");
                try {
                    var contents = File.ReadAllText(path);
                    if(contents.Contains(FindInVisualStudioEditorCs)) {
                        contents = contents.Replace(FindInVisualStudioEditorCs, ReplaceInVisualStudioEditorCs);
                        File.WriteAllText(path, contents);
                    }
                } catch {
                    // Don't surface errors to the user, it's confusing, and this
                    // code is mostly a hack for MapStation devs anyway.
                }
                return;
            }
        }
    }
}

/// <summary>
/// Generate alternate csproj files by copying and modifying the .csprojs created by Unity.
/// See also: notes/csproj.md
/// </summary>
public class GenerateFixedCsProjs : AssetPostprocessor
{
    private const string Suffix = "-alternative";
    private static string OnGeneratedCSProject(string path, string contents) {
        var filename = Path.GetFileName(path);

        if(filename == "MapStation.Tools.csproj" || filename == "MapStation.Tools.Editor.csproj") {
            // var outputPath = path.Substring(0, path.Length - 7) + $"{Suffix}.csproj";
            var outputPath = path[0..^7] + $"{Suffix}.csproj";
            var outputContent = contents;
            outputContent = outputContent.Replace("<TargetFramework>netstandard2.1</TargetFramework>", "<TargetFramework>net471</TargetFramework>");
            outputContent = outputContent.Replace("<NoWarn>0169;USG0001</NoWarn>", "<NoWarn>0169;USG0001;CS0649;CS0169</NoWarn>");
            outputContent = outputContent.Replace("Include=\"MapStation.Common.csproj\"", "Include=\"../MapStation.Common/MapStation.Common.csproj\"");
            outputContent = outputContent.Replace("Include=\"MapStation.Tools.csproj\"", $"Include=\"MapStation.Tools{Suffix}.csproj\"");
            File.WriteAllText(outputPath, outputContent);
        }

        return contents;
    }
}
#endif