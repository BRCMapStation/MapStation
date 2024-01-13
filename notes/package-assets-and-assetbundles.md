## Assets in Unity Packages Assigned to Asset Bundles

For Assets inside a Unity Package (from Package Manager) Unity does not show the UI to assign them to an asset bundle.
But apparently, these assets may still be assigned to bundles internally. This causes missing assets at runtime, because
our plugin never loads these extra bundles.

The solution is removing these assets from bundles.  Assets that are *not* assigned to a bundle will be included
automatically in whatever map bundles reference them.

We can see an asset's bundle assignment in the `.meta` file:

```
  assetBundleName: map template
  assetBundleVariant: sup
```

To unassign, change it to:

```
  assetBundleName: 
  assetBundleVariant: 
```

If necessary, we can write a script to automate this, searching the entire `MapStation.Tools` directory.
