param(
    # If true, builds registry to be hosted as local fileserver for testing
    [switch]$LocalRegistry = $False,
    [switch]$Clean = $False,
    [switch]$Release = $False
)

#Requires -Version 7.4
$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $true

<#
Validate params and environment
#>

if($LocalRegistry -and $Release) {
    Write-Error 'Cannot combine -LocalRegistry with -Release'
}

if(0 + $(npm --version).Split('.')[0] -lt 10) {
    Write-Error "Requires npm >=10, installed version is $(npm --version)"
}

<#
Set globals
#>

if($LocalRegistry) {
    $PackageRegistryDir = 'Build/PackageRegistry.Local'
} else {
    $PackageRegistryDir = 'Build/PackageRegistry.Publish'
}
if($Release) {
    $Configuration='Release'
} else {
    $Configuration='Debug'
}

<#
Functions
#>

function CreateZip($zipPath) {
    if(Test-Path $zipPath) { Remove-Item $zipPath }
    $zip = [System.IO.Compression.ZipFile]::Open($zipPath, 'Create')
    return $zip
}
function AddToZip($zip, $path, $pathInZip=$path) {
    [System.IO.Compression.ZipFileExtensions]::CreateEntryFromFile($zip, $path, $pathInZip) > $Null
}
function EnsureDir($path) {
    if(!(Test-Path $path)) { New-Item -Type Directory $path > $null }
}

function Clean() {
    if(Test-Path Build) {
        Remove-Item -Recurse Build
    }
}

function CreatePluginZip() {
    $zipPath = "Build/MapStation.Plugin.$Configuration.zip"
    $zip = CreateZip $zipPath

    Push-Location "Build/MapStation.Plugin.$Configuration"
    Get-ChildItem -Recurse './' -Exclude '*.pdb' | ForEach-Object {
        $path = ($_ | Resolve-Path -Relative).Replace('.\', '')
        AddToZip $zip $_.FullName $path
    }
    Pop-Location
    Push-Location "Thunderstore"
    Get-ChildItem -Recurse './' | ForEach-Object {
        $path = ($_ | Resolve-Path -Relative).Replace('.\', '')
        AddToZip $zip $_.FullName $path
    }
    Pop-Location

    $zip.Dispose()
}

function CreateEditorZip() {
    $zipPath = 'Build/MapStation.Editor.zip'
    $zip = CreateZip $zipPath

    # Start with everything tracked by git
    $trackedFiles = $( git ls-files --cached --exclude-standard 'MapStation.Editor' )
    # Exclude additional files
    $exclusions = @(
        # Has our dev-only `#if` defines
        'MapStation.Editor/Assets/csc.rsp*'
        # Git is configured for local development, so we manually put the
        # correct release manifest into the zip later
        'MapStation.Editor/Packages/manifest*.json'
    )
    $exclusionFlags = @($exclusions | ForEach-Object { "--exclude=$_" })
    $excluded = $( git ls-files --ignored -c `
        @exclusionFlags `
        'MapStation.Editor'
    )
    $files = $trackedFiles | Where-Object { $excluded -notcontains $_ }

    foreach($file in $files) {
        AddToZip $zip $file $file
    }

    # Add correct package manager manifest
    AddToZip $zip 'MapStation.Editor/Packages/manifest-releaseregistry.json' 'MapStation.Editor/Packages/manifest.json'

    $zip.Dispose()
}

function CreatePackageTarballs() {
    Push-Location MapStation.Common
    $dest = "../$PackageRegistryDir/tarballs/com.brcmapstation.common/-"
    EnsureDir $dest
    npm pack --pack-destination $dest
    Pop-Location

    Push-Location MapStation.Tools
    $dest = "../$PackageRegistryDir/tarballs/com.brcmapstation.tools/-"
    EnsureDir $dest
    npm pack --pack-destination $dest
    Pop-Location
}

function WritePackageRegistryJson() {
    $version = '0.0.1'
    $searchJson = @{
        objects = @()
    }
    function AddPackage($packageName, $versions) {
        $searchJson.objects += @{
            package = @{
                name = $packageName
                version = $version
            }
        }

        if($LocalRegistry) {
            $urlPrefix = 'http://localhost:8000'
        } else {
            $urlPrefix = 'https://brcmapstation.github.io/registry'
        }

        $latestVersion = ($versions | ForEach-Object {
            new-object system.version $version
        } | sort-object -descending)[0].ToString()

        $json = @{
            name = $packageName
            'dist-tags' = @{
                latest = $latestVersion
            }
            versions = @{
            }
        }
        foreach($version in $versions) {
            $json.versions.$version = @{
                name = $packageName
                version = $version
                displayName = "BRC MapStation Common"
                description = "Mapping tools for Bomb Rush Cyberfunk"
                dist = @{
                    shasum = $( Get-FileHash -Algorithm Sha1 -Path "$PackageRegistryDir/tarballs/$packageName/-/$packageName-$version.tgz" ).Hash
                    tarball = "$urlPrefix/tarballs/$packageName/-/$packageName-$version.tgz"
                }
            }
        }
        ConvertTo-Json -Depth 10 $json > $PackageRegistryDir/$packageName
    }

    Get-ChildItem $PackageRegistryDir/tarballs/* | ForEach-Object {
        $packageName = Split-Path -Leaf $_
        $versions = Get-ChildItem $PackageRegistryDir/tarballs/$packageName/- `
        | ForEach-Object {
            (split-path -leafbase $_.name).split('-')[1]
        }
        AddPackage $packageName $versions
    }

    $searchJson.total = $searchJson.objects.Count
    EnsureDir $PackageRegistryDir/-/v1
    ConvertTo-Json -Depth 10 $searchJson > $PackageRegistryDir/-/v1/search
}

$RestoreToCwd = Get-Location
Set-Location $PSScriptRoot/..
try {

    if($Release -or $Clean) {
        Clean
    }
    dotnet build -c $Configuration
    CreatePluginZip
    CreateEditorZip
    CreatePackageTarballs
    WritePackageRegistryJson

} finally {
    Set-Location $RestoreToCwd
}
