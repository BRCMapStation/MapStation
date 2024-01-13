This subdirectory should match the directory structure of the AssetRipper project.

`AssetRipperProject/Assets/Foo/bar -> <this directory>/Foo/bar`

Sometimes our prefabs need to reference assets from the base game.  We know these
assets will be available at runtime loaded from common AssetBundles, but we need
copies in our Unity project to reference.
