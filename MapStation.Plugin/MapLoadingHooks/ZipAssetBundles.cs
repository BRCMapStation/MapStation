using System.Collections.Generic;

namespace MapStation.Plugin;

/// <summary>
/// API to expose assetbundles from zip files to the game.
/// </summary>
public class ZipAssetBundles {
    public static ZipAssetBundles Instance;

    public Dictionary<string, ZipAssetBundle> Bundles = new ();
}

public class ZipAssetBundle {
    public string zipPath;
    public ZipBundleType bundleType;
}

public enum ZipBundleType {
    ASSETS,
    SCENE
}
