cd ../
Write-Output "====== Restoring Assembly GUIDs ======="
$metaPath = ".\Winterland.Editor\Assets\Scripts\Winterland.Common.dll.meta"
$metaExists = Test-Path $metaPath
if (-not $metaExists){
    git checkout $metaPath
}

$metaPath = ".\Winterland.Editor\Assets\Scripts\EasyDecal.Runtime.dll.meta"
$metaExists = Test-Path $metaPath
if (-not $metaExists){
    git checkout $metaPath
}

$metaPath = ".\Winterland.Editor\Assets\Scripts\Winterland.MapStation.Common.dll.meta"
$metaExists = Test-Path $metaPath
if (-not $metaExists){
    git checkout $metaPath
}
Write-Output "====== Building Assemblies ======="

dotnet build