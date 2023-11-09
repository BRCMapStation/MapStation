#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// Settings to control the editor experience for mapping, does not affect how the
/// map plays.
[FilePath("Assets/BRCMapToolkitPreferences.asset", FilePathAttribute.Location.ProjectFolder)]
[CreateAssetMenu]
public class Preferences : ScriptableSingleton<Preferences> { 
    public GeneralPreferences general = new(); 
    public GrindEditingPreferences grinds = new();

    private void OnEnable() {
        // Enable inspector, because ScriptableSingleton defaults this off
        hideFlags &= ~HideFlags.NotEditable;
    }
    
    private void OnValidate() {
        // Auto-save, because ScriptableSingleton requires explicit save for
        // some reason.
        if(EditorUtility.IsDirty(this)) {
             Save(true);
       }
    }
}
#endif
