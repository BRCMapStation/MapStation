Explaining our different assemblies, why they exist, justifying their configuration.

### MapStation.Common

- Unity Editor and BepInEx
- Has our MonoBehaviours used in scenes, configured in editor, loaded in BepInEx
- Build w/`dotnet build` *and* Unity Editor asmdef
- Use `#if BEPINEX` to conditionally enable / disable in BepInEx / Editor

### MapStation.Plugin

- BepInEx only
- Build w/`dotnet build` only
- link against `MapStation.Common`

### MapStation.Tools

- Unity Editor only
- asmdef limits to platforms: Editor&Win32&Win64
  - *must* limit to access Windows APIs. If we leave all platforms enabled, we lose access to windows-only APIs, such as `Microsoft.Win32.Registry`
  - *must* include Win32&Win64 to declare MonoBehaviour components such as `BRCMap`. If we limit to "Editor", they're unavailable

### MapStation.Tools.Editor

- Unity Editor only
- asmdef limits to platform: Editor
- Used for `Editor` subdirectories
- Easier than wrapping files in `#if UNITY_EDITOR`