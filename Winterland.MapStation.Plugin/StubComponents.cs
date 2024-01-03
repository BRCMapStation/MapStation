using UnityEngine;

/// Empty components at runtime so that BepInEx console does not log warnings
/// about missing components.
/// These components are Editor-only, implemented in the Mapping Toolkit.
/// Ideally, we should strip them from maps on save.

namespace MapStation.Components {
    public class Grind : MonoBehaviour {}
}