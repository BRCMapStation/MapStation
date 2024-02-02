<#
Copy all non-MapStation dependencies from manifest.json to manifest-localregistry.json and manifest-releaseregistry.json
Keeps version numbers in sync so that our dev editor project has same deps as published editor projects.
#>

#Requires -Version 7.4
$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $true

. $PSScriptRoot/common.ps1

$RestoreCwd = Get-Location
Set-Location $PSScriptRoot/..
try {

    $manifest = GetJson './MapStation.Editor/Packages/manifest.json'
    $dependencies = $manifest.dependencies
    $dependencies = $dependencies | Select-Object -ExcludeProperty 'com.brcmapstation.*'

    PatchJson './MapStation.Editor/Packages/manifest-localregistry.json' {
        $_.dependencies = $_.dependencies | Select-Object -Property 'com.brcmapstation.tools'
        foreach ($property in $dependencies.psobject.Properties) {
            $_.dependencies | Add-Member -NotePropertyName $property.Name -NotePropertyValue $property.Value
        }
    }
    PatchJson './MapStation.Editor/Packages/manifest-releaseregistry.json' {
        $_.dependencies = $_.dependencies | Select-Object -Property 'com.brcmapstation.tools'
        foreach ($property in $dependencies.psobject.Properties) {
            $_.dependencies | Add-Member -NotePropertyName $property.Name -NotePropertyValue $property.Value
        }
    }

} finally {
    Set-Location $RestoreCwd
}
