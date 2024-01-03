cd ../
Write-Output "====== Updating Submodules ======="
git submodule update --init --recursive
Write-Output "====== Restoring Assembly GUIDs ======="

$metaPath = ".\MapStation.Editor\Assets\Scripts\EasyDecal.Runtime.dll.meta"
git checkout $metaPath

$metaPath = ".\MapStation.Editor\Assets\Scripts\CommonAPI.dll.meta"
git checkout $metaPath

$metaPath = ".\MapStation.Editor\Assets\Scripts\SlopCrew.API.dll.meta"
git checkout $metaPath

Write-Output "====== Building Assemblies ======="

dotnet build