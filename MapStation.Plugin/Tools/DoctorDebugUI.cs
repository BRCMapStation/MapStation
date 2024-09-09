using System;
using System.Linq;
using System.Reflection;
using CommonAPI;
using MapStation.Common;
using MapStation.Common.Doctor;
using Reptile;
using UnityEngine;
using static UnityEngine.GUILayout;
using static UnityEngine.GUI;
using static cspotcode.UnityGUI.GUIUtil;
using Object = UnityEngine.Object;

namespace MapStation.Plugin.Tools;

public class DoctorDebugUI : DebugUI.DebugMenu {
    public override string Name => "Map Doctor";
    public override int? Width => widths[widthIndex];
    private int widthIndex = 0;
    private int[] widths = new[] {
        DebugUI.DefaultWidth,
        700,
        1000
    };
    private Analysis analysis;
    private bool[] showDiag;
    private bool[] showGo;
    private Vector2 scrollPosition = Vector2.zero;
    private bool HasUnityExplorer;
    private Assembly unityExplorerAssembly;
    private Type unityExplorerInspectorManagerType;
    private Type unityExplorerUIManagerType;
    private Type unityExplorerCacheObjectBase;
    
    public DoctorDebugUI() {
        StageAPI.OnStagePreInitialization += OnStagePreInitialization;
        HasUnityExplorer = false;
        unityExplorerAssembly = AppDomain.CurrentDomain.GetAssemblies().
            SingleOrDefault(assembly => assembly.GetName().Name == "UnityExplorer.BIE5.Mono");
        if(unityExplorerAssembly != null) {
            unityExplorerInspectorManagerType = unityExplorerAssembly.GetExportedTypes().SingleOrDefault(type => type.FullName == "UnityExplorer.InspectorManager");
            unityExplorerUIManagerType = unityExplorerAssembly.GetExportedTypes().SingleOrDefault(type => type.FullName == "UnityExplorer.UI.UIManager");
            unityExplorerCacheObjectBase = unityExplorerAssembly.GetExportedTypes().SingleOrDefault(t => t.Name == "CacheObjectBase");
            HasUnityExplorer = unityExplorerInspectorManagerType != null;
        }
    }
    
    public override void OnGUI() {
        GUIStyle wrappedLabel = new GUIStyle(skin.label) {
            wordWrap = true
        };
        GUIStyle boldLabel = new GUIStyle(skin.label) {
            fontStyle = FontStyle.Bold
        };
        Label(Doctor.AboutMe);
        if (Button("Expand")) {
            widthIndex = (widthIndex + 1) % widths.Length;
        }
        if (Button("Analyze")) {
            if (MapDatabase.Instance.maps.TryGetValue(Core.Instance.BaseModule.CurrentStage, out var pluginMapEntry)) {
                analysis = Doctor.Analyze(pluginMapEntry.Properties);
                showDiag = new bool[analysis.diagnostics.Count];
                showGo = new bool[analysis.gameObjects.Count + 1];
            }
        }
        if (analysis != null) {
            Label($"Analysis found {analysis.diagnostics.Count} problems.", boldLabel);
            using (ScrollView(ref scrollPosition)) {
                var di = -1;
                var goi = 0;
                if (analysis.diagnosticsWithoutTarget.Count > 0) {
                    Label("Misc");
                    using (Indent(increment: 2, apply:true)) {
                        foreach (var diagnostic in analysis.diagnosticsWithoutTarget) {
                            DrawDiagnostic(diagnostic);
                        }
                    }
                }
                foreach(var pair in analysis.gameObjects) {
                    goi++;
                    var gameObject = pair.Key;
                    var diagnostics = pair.Value;
                    showGo[goi] = Toggle(showGo[goi], $"{expandCollapseIcon(showGo[goi])} GameObject: {diagnostics[0].TargetPath}", skin.label);
                    if(showGo[goi]) {
                        using (Indent(apply:true)) {
                            if (HasUnityExplorer && Button("UnityExplorer")) {
                                OpenInUnityExplorer(gameObject);
                            }
                            foreach (var diagnostic in diagnostics) {
                                DrawDiagnostic(diagnostic);
                            }
                        }
                    }
                }
                
                void DrawDiagnostic(Diagnostic diagnostic) {
                    di++;
                    showDiag[di] = Toggle(showDiag[di], $"{expandCollapseIcon(showDiag[di])} {diagnostic.Message}", wrappedLabel);
                    if (showDiag[di]) {
                        using (Indent(apply:true)) {
                            Label(diagnostic.Details == null ? "<no details>" : diagnostic.Details);
                        }
                    }
                }
            }
        }
    }

    private char expandCollapseIcon(bool expanded) {
        return expanded ? DownTriangle : RightTriangle;
    }

    private void OpenInUnityExplorer(Object obj) {
        // UnityExplorer.InspectorManager.Inspect(theObject);
        var method = unityExplorerInspectorManagerType.GetMethod("Inspect", new Type[] {typeof(object), unityExplorerCacheObjectBase});
        method.Invoke(null, new[] {obj, null});
        method = unityExplorerUIManagerType.GetMethod("set_ShowMenu");
        method.Invoke(null, new object[] {true});
    }

    private void OnStagePreInitialization(Stage newStage, Stage previousStage) {
        // Discard old analysis when a new stage is loaded.
        analysis = null;
    }
}
