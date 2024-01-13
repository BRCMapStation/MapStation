param(
    [switch]$major,
    [switch]$minor,
    [switch]$patch,
    [string]$version,
    [switch]$NoGit
)

#Requires -Version 7.4
$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $true

function PatchJson($path, $fn) {
    $json = Get-Content $path | ConvertFrom-Json
    # Trick to call $fn and set the automatic $_ var
    ForEach-Object -Process $fn -InputObject $json
    $json | ConvertTo-Json -Depth 99 | Set-Content $path
}

$RestoreCwd = Get-Location
Set-Location $PSScriptRoot/..
try {

    # Abort if git shows uncommitted changes
    if($Null -ne $(git status --untracked-files=no --porcelain=v1)) {
        Write-Error "Git status shows modified files. This script cannot commit a new version while there are uncommitted, modified files."
    }

    # Delegate to npm for versioning logic, it's easier
    $npmFlags = @(
        if($major) {'major'}
        if($minor) {'minor'}
        if($patch) {'patch'}
        if($version) {$version}
    )
    if($npmFlags.Count -eq 0) {
        Write-Error 'Missing flags'
    }
    npm version --no-git-tag-version @npmFlags

    # Copy version number to other files
    $version = (Get-Content package.json | ConvertFrom-Json).version

    PatchJson './MapStation.Common/package.json' {
        $_.version = $version
    }
    PatchJson './MapStation.Tools/package.json' {
        $_.dependencies.'com.brcmapstation.common' = $version
        $_.version = $version
    }
    PatchJson './thunderstore/manifest.json' {
        $_.version_number = $version
    }
    PatchJson './MapStation.Editor/Packages/manifest-localregistry.json' {
        $_.dependencies.'com.brcmapstation.tools' = $version
    }
    PatchJson './MapStation.Editor/Packages/manifest-releaseregistry.json' {
        $_.dependencies.'com.brcmapstation.tools' = $version
    }

    if(-not $NoGit) {
        git add `
            './package.json' `
            './MapStation.Common/package.json' `
            './MapStation.Tools/package.json' `
            './Thunderstore/manifest.json' `
            './MapStation.Editor/Packages/manifest-localregistry.json' `
            './MapStation.Editor/Packages/manifest-releaseregistry.json'
        git commit -m "v$version"
        git tag $version

        Write-Host -ForegroundColor Green "Don't forget to 'git push --tags'"
    }

} finally {
    Set-Location $RestoreCwd
}