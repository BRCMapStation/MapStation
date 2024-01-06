using System;
using UnityEngine;

/// Settings to control the editor experience for mapping, does not affect how the
/// map plays.
[Serializable]
public class GeneralPreferences {

    // TODO rethink this.
    // Do we continue relying on BepInEx environment variable?
    // Do we let mappers set this path via UI?
    [NonSerialized]
    public string mapDirectory;
}