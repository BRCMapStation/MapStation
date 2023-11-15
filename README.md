# MilleniumWinterland
## Building
To build the project you will need to have set your `BRCPath` environment variable to the installation directory of Bomb Rush Cyberfunk. E.g. `E:\Steam\steamapps\common\BombRushCyberfunk`. The project will pull the required referenced assemblies from the game from here.

You will also need a `BepInExDirectory` env variable, pointing to your BepInEx installation. E.g. `C:\Users\(User)\AppData\Roaming\r2modmanPlus-local\BombRushCyberfunk\profiles\(Profile)\BepInEx`. 
When building the plugin the output assemblies will be automatically copied there under a `MilleniumWinterland` folder.

A Unity Project `Winterland.Editor` is included with the asset bundles the mod utilizes. You will need to build the `Winterland.Common` project first as it uses components from it, it will be automatically copied to the Unity project when built, you can do this from the `MilleniumWinterland.sln` solution file.
To build the asset bundles, with the project open head to `BRC -> Build Assets`. Provided your `BepInExDirectory` env variable is set the bundles will be automatically copied to the plugin folder so you can test the changes in-game right away.

## Unity Project
Prefabs saved under `Assets/Stage Additions` will be automatically exported to a `stages` folder with the prefab name as the filename when you use `Build Assets`. If the name matches the lowercase internal name of one of the game's stages (hideout, osaka, square, etc.) the prefab will be automatically instantiated into the stage in-game.
