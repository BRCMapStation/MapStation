Thunderstore / r2 has a very confusing way of extracting .zips into your BepInEx profile.

Certain, special directory names in the .zip will extract their contents into special locations.

Everything else gets flattened out to the root directory of your mod.

*We hit this with Winterland, where we put assetbundles into a `plugins/SlopBrew-MilleniumWinterland/AssetBundles/*` subdirectory,
and R2 extracted them to the root of our plugin instead: `plugins/SlopBrew-MilleniumWinterland/*`.*

Keep it simple; put everything in the root of the zip.

The only useful exception, IMO: `config` will extract into `BepInEx/config`. This is useful for putting default config files
into the `config` directory, which is *outside* of your plugin's directory!

https://github.com/ebkr/r2modmanPlus/wiki/Structuring-your-Thunderstore-package
