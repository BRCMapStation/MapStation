## Problem:

MapStation uses unity package manager, thus uses an asmdef, thus the assembly is
*not* named Assembly-CSharp, it's `MapStation.Tools`

MonoBehaviours in scenes/prefabs save into assetbundles referencing the
assembly name `MapStation.Tools`.  Unity at runtime will try to instantiate from
that assembly.

## Solution

Create an assembly that redirects all types to Assembly-CSharp

---

This code snippet can generate a list of all types from the Reptile namespace.

```
public static class DumpAll {
    public static void Dump() {
        foreach(var asm in AppDomain.CurrentDomain.GetAssemblies()) {
            foreach(var type in asm.GetTypes()) {
                if (type.Namespace == "Reptile") {
                    Debug.LogFormat("Found {0} {1}", type.AssemblyQualifiedName, type.Assembly.Location);
                }
            }
        }
    }
}

