using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Reptile;

namespace MapStation.Plugin;

/// <summary>
/// Hacks to extend the `Stage` enumeration with additional values.
/// Companion to `StagePatch`
/// </summary>
public static class StageEnum {

    // "Maps" are custom stages, not vanilla
    public const Stage FirstMapId = (Stage)15;
    public const string MapNamePrefix = "mapstation/";
    public const int MapNamePrefixLength = 11;

    public const Stage FirstVanillaStage = Stage.NONE;

    static StageEnum() {
        if(MapNamePrefix.Length != MapNamePrefixLength) {
            throw new System.Exception($"{nameof(StageEnum)}{nameof(MapNamePrefixLength)} is wrong.");
        }
    }

    public static string GetMapName(Stage id) {
        return MapNames[id];
    }
    public static Stage GetMapId(string internalName) {
        return MapIds[internalName];
    }

    /// <summary>
    /// Removes the mapstation prefix from a stage internal name if it has one. Otherwise returns the string unchanged.
    /// </summary>
    public static string RemovePrefixFromInternalName(string prefixedInternalName) {
        var slashIndex = prefixedInternalName.IndexOf("/");
        if (slashIndex < 0)
            return prefixedInternalName;
        return prefixedInternalName.Substring(slashIndex + 1);
    }

    public static Stage HashMapName(string internalName) {

        // Vanilla BRC enum uses values -1 through 14 (inclusive), 16 distinct values
        // 16 values = 4 bits, 28 bits remaining for map ID hashes
        // Take first 28 bits of a sha512sum, then use simple addition to shift it outside the reserved range of -1 through 14

        // Despite using a cryptographically strong hashing algorithm, this is *not* secure because we are using
        // relatively few bits of the hash.
        var shaM = new SHA512Managed();
        var result = shaM.ComputeHash(Encoding.UTF8.GetBytes(internalName));
        // Get first 27 bits of hash as a number from 0 to 0x07FFFFFF
        var value = BitConverter.ToUInt32(result, 0); // first 4 bytes to unsigned int
        value &= 0xFFFFFFF0; // mask off first 28 bits
        value >>= 4; // shift right to make it start at 0
        return (Stage)((int)FirstMapId + value);
    }

    public static Stage AddMapName(string internalName) {
        var id = HashMapName(internalName);
        AddMapName(id, internalName);
        return id;
    }

    public static void AddMapName(Stage id, string internalName) {
        MapNames.Add(id, internalName);
        MapIds.Add(internalName, id);
    }
    public static void RemoveMapName(string internalName) {
        MapNames.Remove(MapIds[internalName]);
        MapIds.Remove(internalName);
    }

    public static bool IsValidMapId(Stage id) {
        // Stage is a signed int, so a custom map is either a positive number higher than or a negative number lower
        // than all the vanilla IDs.
        return id >= FirstMapId || id < FirstVanillaStage;
    }

    public static bool IsKnownMapId(Stage id) {
        return MapNames.ContainsKey(id);
    }

    // TODO make these private or read-only, but that's annoying
    public static readonly Dictionary<string, Stage> MapIds = new ();
    public static readonly Dictionary<Stage, string> MapNames = new ();
}
