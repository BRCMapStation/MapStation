$_pwd = $pwd
try {
    cd $PSScriptRoot/..
    Write-Output "====== Updating Submodules ======="
    git submodule update --init --recursive
    Write-Output "====== Restoring Assembly GUIDs ======="

    $metaPath = ".\MapStation.Editor\Assets\EasyDecal.Runtime.dll.meta"
    git checkout $metaPath

    $metaPath = ".\MapStation.Editor\Assets\CommonAPI.dll.meta"
    git checkout $metaPath

    $metaPath = ".\MapStation.Editor\Assets\SlopCrew.API.dll.meta"
    git checkout $metaPath

    Write-Output "====== Building Assemblies ======="

    dotnet build
} finally {
    cd $_pwd
}
