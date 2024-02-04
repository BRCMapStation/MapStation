public class MapBuilderStatus {
    /// <summary>
    /// True when we are building asset bundles, so `OnValidate` logic intended
    /// for interactive editing can be skipped.
    /// </summary>
    public static bool IsBuilding = false;

}