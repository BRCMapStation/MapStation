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

$RegistryRepo = 'https://github.com/BRCMapStation/PackageRegistry'
if($LocalRegistry) {
    $PackageRegistryDir = 'Build/PackageRegistry.Local'
    $PackageRegistryUrl = 'http://localhost:8000'
} else {
    $PackageRegistryDir = 'Build/PackageRegistry.Publish'
    $PackageRegistryUrl = 'https://brcmapstation.github.io/PackageRegistry'
}
if($Release) {
    $Configuration='Release'
} else {
    $Configuration='Debug'
}
$version = (Get-Content ./package.json | ConvertFrom-Json).version

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
    $zipPath = "Build/MapStation.Editor-$version.zip"
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

function ClonePackageRegistry() {
    git clone $RegistryRepo 'Build/PackageRegistry.Publish'
}

function CreatePackageTarballs() {
    <#
    package.json "files" array tells npm what to include in these tarballs.
    For example, this is how we exclude `bin~` and `obj~`
    #>

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

    return @{
        'com.brcmapstation.tools' = 'MapStation.Tools/package.json'
        'com.brcmapstation.common' = 'MapStation.Common/package.json'
    }
}

function WritePackageRegistryJson($pkgPaths) {
    $searchJson = @{
        objects = @()
    }
    function AddPackage($packageName, $versions, $packageJson) {
        $latestVersion = ($versions | ForEach-Object {
            new-object system.version $_
        } | sort-object -descending)[0].ToString()

        $json = @{
            name = $packageName
            'dist-tags' = @{
                latest = $latestVersion
            }
            versions = @{
            }
        }
        $pkg = Get-Content $pkgPaths.$packageName | ConvertFrom-Json
        foreach($version in $versions) {
            $searchJson.objects += @{
                package = @{
                    name = $packageName
                    version = $version
                }
            }

            $json.versions.$version = @{
                name = $packageName
                version = $version
                displayName = $pkg.displayName
                description = $pkg.description
                dist = @{
                    shasum = $( Get-FileHash -Algorithm Sha1 -Path "$PackageRegistryDir/tarballs/$packageName/-/$packageName-$version.tgz" ).Hash
                    tarball = "$PackageRegistryUrl/tarballs/$packageName/-/$packageName-$version.tgz"
                }
            }
            if($null -ne $pkg.dependencies) {
                $json.versions.$version.dependencies = $pkg.dependencies
            }
        }
        ConvertTo-Json -Depth 99 $json > $PackageRegistryDir/$packageName
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
    ConvertTo-Json -Depth 99 $searchJson > $PackageRegistryDir/-/v1/search
}

$RestoreToCwd = Get-Location
Set-Location $PSScriptRoot/..
try {

    if($Release -or $Clean) {
        Clean
    }
    if($Release) {
        ClonePackageRegistry
    }
    dotnet build -c $Configuration
    CreatePluginZip
    CreateEditorZip
    $pkgPaths = CreatePackageTarballs
    WritePackageRegistryJson $pkgPaths

} finally {
    Set-Location $RestoreToCwd
}
