// Hack: Exists solely to force this DLL to load.
// To trigger this, other assemblies reference `typeof(AssemblyDependencies)`
// They could reference *anything* in this DLL, doesn't have to be this empty
// class.
namespace MapStation.Common.Dependencies {
    public class AssemblyDependencies {}
}