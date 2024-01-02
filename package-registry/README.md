Unity Package Manager uses the same format and server API as npm.

We can host static files that implement this API -- static JSON responses and
tarball downloads.

This subdirectory is my *incomplete* experiment proving that I can install
packages into Unity from a static fileserver.

## References

- https://github.com/npm/registry/blob/master/docs/REGISTRY-API.md#endpoints
- https://docs.unity3d.com/Manual/upm-scoped.html
- `curl https://registry.npmjs.com/lodash` for sample response from real npm registry, although most of the returned fields can be omitted