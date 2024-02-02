using UnityEngine;
using UnityEditor;
using ch.sycoforge.Decal;
using System;
using System.Reflection;

/// <summary>
/// The paid version of EasyDecal has a nice inspector.
/// We only have the Runtime DLL, so this is a replacement inspector.
///
/// https://assetstore.unity.com/packages/tools/utilities/easy-decal-22867
/// Watch their demo video to see the full inspector in action.
/// </summary>
[CustomEditor(typeof(EasyDecal))]
public class EasyDecalEditor : Editor {
    [SerializeField]
    private EasyDecal decal;
    [SerializeField]
    private EasyDecalEditableFields proxy;

    private Editor proxyEditor = null;

    private void Awake()
    {
        decal = target as EasyDecal;
        proxy = CreateInstance<EasyDecalEditableFields>();
        proxy.decal = decal;
    }

    private void OnDestroy() {
        DestroyImmediate(proxyEditor);
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox(
            "This is a hacky replacement for EasyDecal's inspector, since we only have the runtime DLL.\n" + 
            "Click 'Reset to BRC defaults' to set default values similar to TeamReptile's graffiti spots.",
            MessageType.Warning);
        if(GUILayout.Button("Reset to BRC defaults")) {
            Undo.RecordObject(decal, "Reset EasyDecal to BRC defaults");
            resetToBrcDefaults();
        }
        proxy.readFromEasyDecal();
        CreateCachedEditor(proxy, null, ref proxyEditor);
        EditorGUI.BeginChangeCheck();
        proxyEditor.OnInspectorGUI();
        if(EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(decal, "Modify EasyDecal");
            proxy.writeToEasyDecal();
        }
    }

    private void resetToBrcDefaults() {
        decal.Mask = LayerMask.GetMask("Default", "Water", "Wallrun", "VertSurface");
        decal.Baked = false;
        decal.BakeOnAwake = true;
        decal.FlipNormals = false;
        decal.BackfaceCulling = true;
        decal.Distance = 0.01f;
        decal.AngleConstraint = 89;
        decal.Technique = ProjectionTechnique.Box;
        decal.DeferredFlags = 0;
        decal.Inclusion = ProjectorInclusion.All;
        // transformObserver
        decal.NeedsDynamicGeometry = true;
        // mask
        decal.ProjectionTarget = null;
        decal.Mode = ProjectionMode.SurfaceNormal;
        decal.Source = SourceMode.Material;
        decal.AspectCorrectionMode = AspectMode.None;
        decal.RecursiveMode = LookupMode.Up;
        decal.SkinningQuality = SkinQuality.Auto;

        decal.GetType().GetField("material",
            BindingFlags.NonPublic | BindingFlags.Instance).SetValue(decal, null);

        decal.Atlas = null;
        decal.AtlasRegionIndex = 0;
        decal.MaxDistance = 5;
        decal.ProjectionDistance = 1;
        decal.SmoothNormals = false;
        decal.NormalSmoothFactor = 0;
        decal.NormalSmoothThreshold = 0;
        // resolution
        decal.MultiMeshEnabled = false;
        decal.CullInvisibles = false;
        decal.CullUnreachable = false;
        decal.ShowVertices = false;
        decal.ShowDir = false;
        decal.ShowNormals = false;
        // enableVertexColorFade
        decal.OnlyColliders = true;
        decal.CalculateTangents = false;
        decal.CalculateNormals = false;
        decal.Quality = 0;

        decal.Alpha = 1;
    }
}

/// <summary>
/// We cannot modify EasyDecal to annotate fields and properties to appear in the inspector.
/// Instead inspect this proxy which reads/writes all fields to the underlying EasyDecal.
/// </summary>
[Serializable]
class EasyDecalEditableFields : ScriptableObject {
    internal EasyDecal decal;
    public bool Baked;
    public bool BakeOnAwake;
    public bool FlipNormals;
    public bool BackfaceCulling;
    public float Distance;
    public float AngleConstraint;
    public ProjectionTechnique Technique;
    public DeferredFlags DeferredFlags;
    public ProjectorInclusion Inclusion;
    // public ??? transformObserver;
    public bool NeedsDynamicGeometry;
    public LayerMask Mask;
    public GameObject ProjectionTarget;
    public ProjectionMode Mode;
    public SourceMode Source;
    public AspectMode AspectCorrectionMode;
    public LookupMode RecursiveMode;
    public SkinQuality SkinningQuality;
    public DecalTextureAtlas Atlas;
    public int AtlasRegionIndex;
    public float MaxDistance;
    public float ProjectionDistance;
    public bool SmoothNormals;
    public float NormalSmoothFactor;
    public float NormalSmoothThreshold;
    // public ??? resolution
    public bool MultiMeshEnabled;
    public bool CullInvisibles;
    public bool CullUnreachable;
    public bool ShowVertices;
    public bool ShowDir;
    public bool ShowNormals;
    // public ??? enableVertexColorFade
    public bool OnlyColliders;
    public bool CalculateTangents;
    public bool CalculateNormals;
    public int Quality;
    public float Alpha;
    public Material Material;

    internal void readFromEasyDecal() {
        Baked = decal.Baked;
        BakeOnAwake = decal.BakeOnAwake;
        FlipNormals = decal.FlipNormals;
        BackfaceCulling = decal.BackfaceCulling;
        Distance = decal.Distance;
        AngleConstraint = decal.AngleConstraint;
        Technique = decal.Technique;
        DeferredFlags = decal.DeferredFlags;
        Inclusion = decal.Inclusion;
        NeedsDynamicGeometry = decal.NeedsDynamicGeometry;
        Mask = decal.Mask;
        ProjectionTarget = decal.ProjectionTarget;
        Mode = decal.Mode;
        Source = decal.Source;
        AspectCorrectionMode = decal.AspectCorrectionMode;
        RecursiveMode = decal.RecursiveMode;
        SkinningQuality = decal.SkinningQuality;
        Atlas = decal.Atlas;
        AtlasRegionIndex = decal.AtlasRegionIndex;
        MaxDistance = decal.MaxDistance;
        ProjectionDistance = decal.ProjectionDistance;
        SmoothNormals = decal.SmoothNormals;
        NormalSmoothFactor = decal.NormalSmoothFactor;
        NormalSmoothThreshold = decal.NormalSmoothThreshold;
        MultiMeshEnabled = decal.MultiMeshEnabled;
        CullInvisibles = decal.CullInvisibles;
        CullUnreachable = decal.CullUnreachable;
        ShowVertices = decal.ShowVertices;
        ShowDir = decal.ShowDir;
        ShowNormals = decal.ShowNormals;
        OnlyColliders = decal.OnlyColliders;
        CalculateTangents = decal.CalculateTangents;
        CalculateNormals = decal.CalculateNormals;
        Quality = decal.Quality;
        Alpha = decal.Alpha;
        Material = (Material)decal.GetType().GetField("material",
            BindingFlags.NonPublic | BindingFlags.Instance).GetValue(decal);
    }
    internal void writeToEasyDecal() {
        decal.Baked = Baked;
        decal.BakeOnAwake = BakeOnAwake;
        decal.FlipNormals = FlipNormals;
        decal.BackfaceCulling = BackfaceCulling;
        decal.Distance = Distance;
        decal.AngleConstraint = AngleConstraint;
        decal.Technique = Technique;
        decal.DeferredFlags = DeferredFlags;
        decal.Inclusion = Inclusion;
        decal.NeedsDynamicGeometry = NeedsDynamicGeometry;
        decal.Mask = Mask;
        decal.ProjectionTarget = ProjectionTarget;
        decal.Mode = Mode;
        decal.Source = Source;
        decal.AspectCorrectionMode = AspectCorrectionMode;
        decal.RecursiveMode = RecursiveMode;
        decal.SkinningQuality = SkinningQuality;
        decal.Atlas = Atlas;
        decal.AtlasRegionIndex = AtlasRegionIndex;
        decal.MaxDistance = MaxDistance;
        decal.ProjectionDistance = ProjectionDistance;
        decal.SmoothNormals = SmoothNormals;
        decal.NormalSmoothFactor = NormalSmoothFactor;
        decal.NormalSmoothThreshold = NormalSmoothThreshold;
        decal.MultiMeshEnabled = MultiMeshEnabled;
        decal.CullInvisibles = CullInvisibles;
        decal.CullUnreachable = CullUnreachable;
        decal.ShowVertices = ShowVertices;
        decal.ShowDir = ShowDir;
        decal.ShowNormals = ShowNormals;
        decal.OnlyColliders = OnlyColliders;
        decal.CalculateTangents = CalculateTangents;
        decal.CalculateNormals = CalculateNormals;
        decal.Quality = Quality;
        decal.Alpha = Alpha;
        // Requires reflection for some reason, assigning to decal.DecalMaterial does not work
        decal.GetType().GetField("material",
            BindingFlags.NonPublic | BindingFlags.Instance).SetValue(decal, Material);
    }
}
