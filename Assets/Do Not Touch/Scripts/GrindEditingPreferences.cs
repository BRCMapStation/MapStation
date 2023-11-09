using System;
using UnityEngine;

/// Settings to control the editor experience for grinds, does not affect how the
/// map plays.
[Serializable]
public class GrindEditingPreferences {
    [Header("Length of character posture direction gizmo")]
    public float nodePostureDirectionGizmoLength = 1;

    [Help("This line indicates the direction characters will be standing when grinding.\n" +
    "Most grinds point up, ceiling grinds point down, and wall grinds point sideways.\n" +
    "Characters always stand perpendicular to the grind line, but their rotation about the line is controlled by the GrindNodes' rotation.")]
    [HideValueInInspector]
    public bool _dummy;

    // Remove this.
    // At first, I thought I wanted to enable hiding grindlines in the map that
    // are not selected.  However, this is complex, and the hierarchy has better
    // features for showing / hiding objects.
    // For now, this is hard-coded to true.
    [HideInInspector]
    public bool showGizmosForDeselectedPaths = true;

    // Remove this.
    // This was from my prototype where I was drawing capsule gizmos to mimic
    // Unity's capsule collider gizmo, but they were visible even when the object
    // was deselected.
    // This clashes with the red debug shapes.
    [HideInInspector]
    public bool showLineCapsuleGizmos = false;

    // Rethink this.
    // This toggle disables automatically moving the grindline to follow grindnode.
    //
    // I initially though that advanced mapping scenarios would require
    // manually manipulating the GrindLine and its capsule.  For example, maybe
    // a grind's collider needs to reach through a wall (or not) and you require fine-tune control.
    [HideInInspector]
    public bool adjustLinesWhenMovingNodes = true;

    public bool showNodeTransformsInGrindInspector = false;

    public bool autoSelectNewNodes = true;
    [Help("When adding a new node or splitting a grind line, will auto-select the newly-created node.")]
    [HideValueInInspector]
    public bool _dummy2;
}