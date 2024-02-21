# Download pdb2mdb.exe from this gist:
# https://gist.github.com/jbevain/ba23149da8369e4a966f#file-pdb2mdb-exe
# ...which is recommended by BepInEx docs:
# https://github.com/BepInEx/bepinex-docs/blob/master/articles/advanced/debug/plugins_vs.md/#converting-pdb-to-mdb
# pdb2mdb.exe is bundled with Unity, but it doesn't work without extra tricks to force it to run under Unity's mono VM (??)

Invoke-WebRequest -OutFile $PSScriptRoot/pdb2mdb.exe https://gist.github.com/jbevain/ba23149da8369e4a966f/raw/36b3cdd4dd149ab966bbb48141ef8ee2d37c890f/pdb2mdb.exe
