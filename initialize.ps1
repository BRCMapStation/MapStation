[System.Reflection.Assembly]::LoadWithPartialName("System.windows.forms")
Write-Output "Initializing Submodules..."
git submodule update --init --recursive
Write-Output "Restoring Unity GUID for Winterland.Common.dll..."
$metaPath = ".\Winterland.Editor\Assets\Scripts\Winterland.Common.dll.meta"
git checkout $metaPath
Write-Output "====== BepInEx Directory ======="
Write-Output "Example BepInEx Root Directory: C:\Users\(User)\AppData\Roaming\r2modmanPlus-local\BombRushCyberfunk\profiles\(Profile)\BepInEx"
$bepinexPathAlreadySet = $false
if ($env:BepInExDirectory)
{
    $bepinexPathAlreadySet = $true
    Write-Output "Your BepInEx Directory is already set to $env:BepInExDirectory. Click Cancel to leave it unchanged"
}

$foldername = New-Object System.Windows.Forms.FolderBrowserDialog
$foldername.Description = "Locate your BepInEx root Directory."
$foldername.rootfolder = "Desktop"
$foldername.SelectedPath = $env:APPDATA
$dialogResult = $foldername.ShowDialog()

if ($dialogResult -eq "Cancel" -and -not $bepinexPathAlreadySet)
{
    Write-Output "Cancelling as you didn't set your BepInEx directory."
    exit
}

if ($dialogResult -eq "OK")
{
    $path = $foldername.SelectedPath
    [Environment]::SetEnvironmentVariable("BepInExDirectory", $path, "User")
    $env:BepInExDirectory = $path
}

Write-Output "====== Bomb Rush Cyberfunk Directory ======="
Write-Output "Example Bomb Rush Cyberfunk installation Directory: E:\Steam\steamapps\common\BombRushCyberfunk"
$brcPathAlreadySet = $false
if ($env:BRCPath)
{
    $brcPathAlreadySet = $true
    Write-Output "Your Bomb Rush Cyberfunk installation directory is already set to $env:BRCPath. Click Cancel to leave it unchanged"
}
$foldername = New-Object System.Windows.Forms.FolderBrowserDialog
$foldername.Description = "Locate your Bomb Rush Cyberfunk installation Directory."
$foldername.rootfolder = "Desktop"
$dialogResult = $foldername.ShowDialog()

if ($dialogResult -eq "Cancel" -and -not $brcPathAlreadySet)
{
    Write-Output "Cancelling as you didn't set your Bomb Rush Cyberfunk directory."
    exit
}

if ($dialogResult -eq "OK")
{
    $path = $foldername.SelectedPath
    [Environment]::SetEnvironmentVariable("BRCPath", $path, "User")
    $env:BRCPath = $path
}

Write-Output "====== Building Common Scripts ======="

cd Winterland.Common
dotnet build

Write-Output "====== Building Plugin ======="

cd ../
cd Winterland.Plugin
dotnet build

cd ../

read-host “Press ENTER to continue”