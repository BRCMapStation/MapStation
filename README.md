# MilleniumWinterland
## Building
The first time before doing anything else you should run the `initialize.ps1` script with Powershell. It will ask you for the directories the project requires, and build everything for you.

Once that's done, you can open the `Winterland.Editor` Unity project, using Unity 2021.3.27f1, head to `BRC -> Build Assets` (or just press F5), which will build all assets the plugin requires.

Everytime you build assets they will be automatically copied to your `BepInEx/plugins/MilleniumWinterland` folder, so all you need to do is launch the game to see your changes.

## Unity Project
Prefabs saved under `Assets/Stage Additions` will be automatically exported to a `stages` folder with the prefab name as the filename when you use `Build Assets`. If the name matches the lowercase internal name of one of the game's stages (hideout, osaka, square, etc.) the prefab will be automatically instantiated into the stage in-game.
