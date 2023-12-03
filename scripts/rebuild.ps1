cd ../
Write-Output "====== Updating Submodules ======="
git submodule update --init --recursive
Write-Output "====== Restoring Assembly GUIDs ======="

$metaPath = ".\Winterland.Editor\Assets\Scripts\Winterland.Common.dll.meta"
git checkout $metaPath

$metaPath = ".\Winterland.Editor\Assets\Scripts\EasyDecal.Runtime.dll.meta"
git checkout $metaPath

$metaPath = ".\Winterland.Editor\Assets\Scripts\Winterland.MapStation.Common.dll.meta"
git checkout $metaPath

Write-Output "====== Building Assemblies ======="

dotnet build