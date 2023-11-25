# MilleniumWinterland
## Building
The first time before doing anything else you should run the `initialize.ps1` script with Powershell. It will ask you for the directories the project requires, and build everything for you.

Once that's done, you can open the `Winterland.Editor` Unity project, using Unity 2021.3.27f1, head to `BRC -> Build Assets` (or just press F5), which will build all assets the plugin requires.

Everytime you build assets they will be automatically copied to your `BepInEx/plugins/MilleniumWinterland` folder, so all you need to do is launch the game to see your changes.

## Unity Project
Prefabs saved under `Assets/Stage Additions` will be automatically exported to a `stages` folder with the prefab name as the filename when you use `Build Assets`. If the name matches the lowercase internal name of one of the game's stages (hideout, osaka, square, etc.) the prefab will be automatically instantiated into the stage in-game.

Use `BRC -> Build Assets` or press F5 to build the assets and update the plugin with your changes. This will also automatically run `Update Plugin` if it detects that the plugin build is out of date.

Use `BRC -> Update Plugin` or press F6 to rebuild the plugin code. This runs the powershell script located at `scripts/rebuild.ps1`. This will be greyed out if the plugin is already up to date.

## Testing In-Game
Press F5 In-Game to reload the plugin assets and the current stage, to test changes in realtime.

When launching for the first time a `BepInEx/config/Winterland.Plugin.cfg` config file will be created. There is a QuickLaunch option that, when set to true, will make the game skip all intros and load straight into Millenium Square.

### Attaching a debugger

You can attach a debugger to the game if you first convert it to a debug build and download pdb2mdb.

The script `./scripts/convert-to-debug-build.ps1` will convert your BRC installation to a debug build.

Download pdb2mdb to `./scripts/pdb2mdb.exe`.

Then build the solution using the "Debug" configuration.

You should see `.dll.mdb` files in your `BepInEx/plugins/MilleniumWinterland`. The game should show "Development Build" in the lower-right corner. You should be able to "Attach Unity Debugger" in Visual Studio, or equivalent in other IDEs.
