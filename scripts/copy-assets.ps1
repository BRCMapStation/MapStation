cd ../
$bepinexDirectory = $env:BepInExDirectory
$destinationPath = "$bepinexDirectory\plugins\MapStation"
Copy-Item -Path "Assets\*" -Destination $destinationPath -Recurse