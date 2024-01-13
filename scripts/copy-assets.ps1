$RestoreCwd = Get-Location
Set-Location $PSScriptRoot/..
try {
    $bepinexDirectory = $env:BepInExDirectory
    $destinationPath = "$bepinexDirectory\plugins\MapStation"
    Copy-Item -Path "Assets\*" -Destination $destinationPath -Recurse
} finally {
    Set-Location $RestoreCwd
}