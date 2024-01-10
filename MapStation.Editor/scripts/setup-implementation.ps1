$ErrorActionPreference = 'Stop'
trap  {
    Write-Host ""
    Write-Host -ForegroundColor Red $_
    Write-Host ""
    SetupFailed
}

function SetupFailed {
    Write-Host -ForegroundColor Red "Setup failed!"
    PauseBeforeQuit
}
function PauseBeforeQuit {
    Read-Host "Press Enter to quit"
    exit
}

$EditorProject = Split-Path -Parent $PSScriptRoot

# Must match PathDetection.cs!
$RegistryKey = "HKCU:\Software\BRCMapStation\MapStation"
$RegistryValueBRCPath = "BRCPath"

$BRCPath = $null
try {
    $BRCPath = Get-ItemPropertyValue -path $RegistryKey -name $RegistryValueBRCPath
} catch { }

function GetEasyDecalPath() {
    $BRCPath + "/Bomb Rush Cyberfunk_Data/Managed/EasyDecal.Runtime.dll"
}

$EasyDecalPath = GetEasyDecalPath
if($BRCPath -eq $null -or -not (Test-Path $EasyDecalPath)) {
    Write-Host "Unable to detect path to your Bomb Rush Cyberfunk installation"
    Write-Host ""
    Write-Host "Have you launched BRC with the MapStation plugin yet?"
    Write-Host "Try installing the plugin and launching modded BRC first, then run this script again."
    Write-Host ""

    $decision = $Host.UI.PromptForChoice("", "What would you like to do?", ('&Find installation manually', '&Quit'), 1)
    $findManually = $decision -eq 0
    if($decision -eq 1) {
        SetupFailed
    }
} else {
    Write-Host "`nDetected Bomb Rush Cyberfunk installed at: $BRCPath`n"
    $decision = $Host.UI.PromptForChoice("", "Is this correct?", ('&Yes','&No', '&Quit'), 0)
    $findManually = $decision -eq 1
    if($decision -eq 2) {
        exit
    }
}

if($findManually) {
    Write-Host ""
    Read-Host "Press Enter to locate your 'Bomb Rush Cyberfunk.exe'"
    [System.Reflection.Assembly]::LoadWithPartialName("System.windows.forms") > $null
    $dialog = New-Object System.Windows.Forms.OpenFileDialog
    $parent = New-Object System.Windows.Forms.Form -Property @{TopMost = $true }
    $dialogResult = $dialog.ShowDialog($parent)

    if ($dialogResult -eq "Cancel") {
        exit
    }

    $BRCExe = $dialog.Filename
    $BRCPath = Split-Path $BRCExe -Parent

    $dialog.Dispose()
    $parent.Dispose()

    Write-Host "Executable: $BRCExe"
    Write-Host "Directory:  $BRCPath"
    Write-Host ""
    $decision = $Host.UI.PromptForChoice("", "Is this correct?", ('&Yes','&No'), 0)
    if($decision -eq 1) {
        exit
    }
}

$EasyDecalPath = GetEasyDecalPath

Copy-Item $EasyDecalPath "$EditorProject/Assets"

Write-Host ""
Write-Host -ForegroundColor Green "Setup Complete!"
Read-Host "Press Enter to quit"

