using System.Collections.Generic;
using Reptile;

namespace MapStation.Plugin;

/// <summary>
/// Hacks to extend the `Stage` enumeration with additional values.
/// Companion to `StagePatch`
/// </summary>
public static class StageEnum {

    // "Bundled maps" are not the vanilla BRC maps, they're ones we ship with mapstation, for example the subway
    // station.
    // Named "bundled" instead of "internal" to avoid confusion with internal names

    public const Stage FirstBundledMapId = (Stage)900;
    public const string BundledMapNamePrefix = "mapstation_bundled/";
    public const int BundledMapNamePrefixLength = 19;

    // "Maps" are any custom maps
    public const Stage FirstMapId = (Stage)1000;
    public const string MapNamePrefix = "mapstation/";
    public const int MapNamePrefixLength = 11;

    private static Stage NextMapId = FirstMapId;

    static StageEnum() {
        if(BundledMapNamePrefix.Length != BundledMapNamePrefixLength) {
            throw new System.Exception($"{nameof(StageEnum)}{nameof(BundledMapNamePrefixLength)} is wrong.");
        }
        if(MapNamePrefix.Length != MapNamePrefixLength) {
            throw new System.Exception($"{nameof(StageEnum)}{nameof(MapNamePrefixLength)} is wrong.");
        }
    }

    public static Stage ClaimCustomMapId() {
        return NextMapId++;
    }

    public static string GetMapName(Stage id) {
        return MapNames[id];
    }
    public static Stage GetMapId(string internalName) {
        return MapIds[internalName];
    }

    public static void AddMapName(Stage id, string internalName) {
        MapNames.Add(id, internalName);
        MapIds.Add(internalName, id);
    }
    public static void RemoveMapName(Stage id, string internalName) {
        MapNames.Remove(id);
        MapIds.Remove(internalName);
    }

    public static bool IsValidMapId(Stage id) {
        return id >= FirstMapId;
    }

    public static bool IsKnownMapId(Stage id) {
        return MapNames.ContainsKey(id);
    }

    public static bool IsValidBundledMapId(Stage id) {
        return id >= FirstBundledMapId && id < FirstMapId;
    }

    // TODO make these private or read-only, but that's annoying
    public static readonly Dictionary<string, Stage> MapIds = new ();
    public static readonly Dictionary<Stage, string> MapNames = new ();
    public static readonly Dictionary<string, Stage> BundledMapIds = new ();
    public static readonly Dictionary<Stage, string> BundledMapNames = new ();
}