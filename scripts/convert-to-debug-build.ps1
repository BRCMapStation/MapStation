param($brcInstallDirectory, $unityEditorDirectory)

if(-not $unityEditorDirectory) {
    # Previous patch runs .27.  If you downpatch on Steam, use this
    # $unityEditorDirectory="C:\Program Files\Unity\Hub\Editor\2021.3.27f1\Editor\Data\PlaybackEngines\windowsstandalonesupport\Variations\win64_player_development_mono"
    # Current patch runs .20
    $unityEditorDirectory="C:\Program Files\Unity\Hub\Editor\2021.3.20f1\Editor\Data\PlaybackEngines\windowsstandalonesupport\Variations\win64_player_development_mono"
}
if(-not $brcInstallDirectory) {
    $brcInstallDirectory=$env:BRCPath
}

Write-Host "This script will convert your Bomb Rush Cyberfunk installation into a debug build."
Write-Host "Backup your game before proceeding!  This may break things."
Write-Host ""
Write-Host "Bomb Rush Cyberfunk installation:  $brcInstallDirectory"
Write-Host "Unity Editor:                      $unityEditorDirectory"
Write-Host ""

$confirmation = Read-Host "Are you sure? (type 'yes')"
if($confirmation -ne "yes") {
    Write-Host "Aborting"
    exit
}

xcopy /E /C /Y "$unityEditorDirectory\Data" "$brcInstallDirectory\Bomb Rush Cyberfunk_data"
xcopy /C /Y "$unityEditorDirectory\UnityPlayer.dll" "$brcInstallDirectory\UnityPlayer.dll"
xcopy /C /Y "$unityEditorDirectory\WindowsPlayer.exe" "$brcInstallDirectory\Bomb Rush Cyberfunk.exe"
Add-Content -Path "$brcInstallDirectory\Bomb Rush Cyberfunk_data\boot.config" "`n`nplayer-connection-debug=1"
